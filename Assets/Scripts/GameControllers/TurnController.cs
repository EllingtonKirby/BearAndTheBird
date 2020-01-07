using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour, DoesOnLevelStart
{
    public static TurnController instance;

    private TurnState state = TurnState.USER_TURN;
    private int currentActionPoints;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        EventManager.StartListening(EventNames.UI_USER_END_TURN, EndTurn);
        EventManager.StartListening(EventNames.UI_ENEMY_END_TURN, BeginTurn);

        EventManager.StartListening(EventNames.ENEMY_TURN_COMPLETED, delegate { HandleEnemyTurnEnd(); });
    }

    public void Instantiate()
    {
        BeginTurn(LevelStoreController.instance.GetPlayer());
    }

    public TurnState GetState()
    {
        return state;
    }

    public bool CheckCostOfMoveAgainstActionPointsRemaining(int actionCost)
    {
        return currentActionPoints - actionCost >= 0;
    }

    public void UseActionPoint(int actionCost)
    {
        currentActionPoints -= actionCost;
        LevelStoreController.instance.GetPlayer().SpendActionPoint(actionCost);
    }

    public int GetActionPointsRemaining()
    {
        return currentActionPoints;
    }

    private void BeginTurn(object argument)
    {
        state = TurnState.USER_TURN;

        var player = argument as Player;
        currentActionPoints = player.ActionPointsPerTurn;

        //Could check the action point reserve here, trigger game over if it is 0
    }

    private void EndTurn(object argument)
    {
        if (currentActionPoints > 0)
        {
            UseActionPoint(currentActionPoints);
        }

        HandleEnemyTurnBegin();
    }

    public void HandleEnemyTurnBegin()
    {
        state = TurnState.ENEMY_TURN;

        EventManager.TriggerEvent(EventNames.ENEMY_TURN_START);

        EnemyPlacementController.instance.EstablishEnemyPositions();
        EnemyRosterController.instance.InitTurnIterator();
        EnemyRosterController.instance.TakeEnemyTurn();
    }

    public void HandleEnemyTurnEnd()
    {
        LevelStoreController.instance.GetPlayer().EnemyTurnIncrement();
    }

    public enum TurnState
    {
        USER_TURN,
        ENEMY_TURN
    }
}
