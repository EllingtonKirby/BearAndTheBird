using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public static TurnController instance;

    public GameObject nextTurnGameObject;
    public Text actionPointsText;
    public int defaultActionPoints;
    private Button nextTurnButton;

    private TurnState state = TurnState.USERS_TURN_IN_PROGRESS;
    private int turnCount = 0;
    private int currentActionPoints;
    private bool dirtyState = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        currentActionPoints = defaultActionPoints;
        dirtyState = true;
        nextTurnButton = nextTurnGameObject.GetComponent<Button>();
        nextTurnButton.onClick.AddListener(delegate { EndTurn(); });
        SetActionPointsText();

        EventManager.StartListening(EventNames.END_MOVE, delegate { dirtyState = true; });
        EventManager.StartListening(EventNames.ENEMY_TURN_COMPLETED, delegate { HandleEnemyTurnEnd(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (dirtyState)
        {
            CheckState();
            switch (state)
            {
                case TurnState.USERS_TURN_IN_PROGRESS:
                    Debug.Log("Users turn");
                    HandleInProgressTurn();
                    break;
                case TurnState.NO_MORE_ACTION_POINTS:
                    HandleNoMoreActionPointsTurn();
                    break;
                case TurnState.ENEMY_TURN_IN_PROGRESS:
                    Debug.Log("Enemy turn");
                    break;
            }
            dirtyState = false;
        }
    }

    public TurnState GetState()
    {
        return state;
    }

    public void CheckState()
    {
        if (turnCount % 2 == 1)
        {
            if (currentActionPoints == 0 && state != TurnState.ENEMY_TURN_IN_PROGRESS)
            {
                state = TurnState.ENEMY_TURN_IN_PROGRESS;
                HandleEnemyTurnBegin();
            }
        }
        else if (currentActionPoints == 0)
        {
            state = TurnState.NO_MORE_ACTION_POINTS;
        }
        else
        {
            state = TurnState.USERS_TURN_IN_PROGRESS;
        }
    }

    private void EndTurn()
    {
        if (currentActionPoints > 0)
        {
            UseActionPoint(currentActionPoints);
        }
        dirtyState = true;
        turnCount += 1;
        EventManager.TriggerEvent(EventNames.END_TURN);
    }

    public bool CheckCostOfMoveAgainstActionPointsRemaining(int actionCost)
    {
        return currentActionPoints - actionCost >= 0;
    }

    public void UseActionPoint(int actionCost)
    {
        currentActionPoints -= actionCost;
        SetActionPointsText();
    }

    public int GetActionPointsRemaining()
    {
        return currentActionPoints;
    }

    void SetActionPointsText()
    {
        actionPointsText.text = "Remaining Action Points: " + currentActionPoints;
    }

    private void HandleNoMoreActionPointsTurn()
    {
    }

    private void HandleInProgressTurn()
    {
        nextTurnGameObject.SetActive(true);
    }

    public void HandleEnemyTurnEnd()
    {
        dirtyState = true;
        currentActionPoints = defaultActionPoints;
        SetActionPointsText();
        turnCount += 1;
    }

    public void HandleEnemyTurnBegin()
    {
        actionPointsText.text = "Enemy Turn";
        nextTurnGameObject.SetActive(false);
        EventManager.TriggerEvent(EventNames.ENEMY_TURN_START);
        EnemyPlacementController.instance.EstablishEnemyPositions();
        EnemyRosterController.instance.InitTurnIterator();
        EnemyRosterController.instance.TakeEnemyTurn();
    }

    public enum TurnState
    {
        USERS_TURN_IN_PROGRESS = 0, NO_MORE_ACTION_POINTS, ENEMY_TURN_IN_PROGRESS
    }
}
