using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UiInteractionController : MonoBehaviour, DoesOnLevelStart
{

    public static UiInteractionController instance;

    private RaycastHit2D[] hitAll;
    private bool stopListenForMouseMove;
    private bool stopListenForMouseClick;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        ListenForMouseEvents();
        ListenForMoveEvents();
        EventManager.StartListening(EventNames.ENEMY_TURN_START, HandleEnemyTurnStart);
        EventManager.StartListening(EventNames.ENEMY_TURN_COMPLETED, HandleEnemyTurnEnd);
    }

    public void Instantiate()
    {
        ListenForMouseEvents();
        ListenForMoveEvents();
    }


    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        hitAll = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
        if (hitAll.Length > 0)
        {
            var isClick = Input.GetMouseButtonDown(0);

            foreach (RaycastHit2D objectHit in hitAll)
            {
                var mouseEventActionProvider = objectHit.collider.gameObject.GetComponent<MouseEventActionProvider>();
                if (mouseEventActionProvider == null)
                {
                    continue;
                }
                if (isClick && !stopListenForMouseClick)
                {
                    HandleMouseDownEvent(mouseEventActionProvider);
                }
                else if (!stopListenForMouseMove)
                {
                    HandleMouseEnterEvent(mouseEventActionProvider);
                }
            }
        }
    }

    private void HandleMouseDownEvent(System.Object data)
    {
        if (data is MouseEventActionProvider)
        {
            var mouseEventData = data as MouseEventActionProvider;
            StartCoroutine(mouseEventData.OnMouseDownAction().Perform());
        }
        else
        {
            Debug.LogError("Received a Mouse event without MouseEventActionProvider");
        }
    }

    private void HandleMouseEnterEvent(System.Object data)
    {
        if (data is MouseEventActionProvider)
        {
            var mouseEventData = data as MouseEventActionProvider;
            StartCoroutine(mouseEventData.OnMouseEnterAction().Perform());
        }
        else
        {
            Debug.LogError("Received a Mouse event without MouseEventActionProvider");
        }
    }

    private void ListenForMoveEvents()
    {
        stopListenForMouseMove = false;
    }

    private void StopListenForMoveEvents()
    {
        stopListenForMouseMove = true;
    }

    private void ListenForMouseEvents()
    {
        stopListenForMouseClick = false;
    }

    private void StopListenForMouseEvents()
    {
        stopListenForMouseClick = true;       
    }

    private void HandleStartMove(System.Object data)
    {
        StopListenForMouseEvents();
    }

    private void HandleEndMove(System.Object data)
    {
        ListenForMouseEvents();
    }

    private void HandleEnemyTurnStart(System.Object data)
    {
        Debug.Log("Enemy turn start from UiInteractionController " + Time.fixedTime);
        StopListenForMouseEvents();
        StopListenForMoveEvents();
    }

    private void HandleEnemyTurnEnd(System.Object data)
    {
        Debug.Log("Enemy turn end from UiInteractionController " + Time.fixedTime);
        ListenForMouseEvents();
        ListenForMoveEvents();
    }
}
