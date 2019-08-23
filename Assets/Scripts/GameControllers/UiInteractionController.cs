using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UiInteractionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ListenForMouseEvents();
        ListenForMoveEvents();
        EventManager.StartListening(EventNames.ENEMY_TURN_START, HandleEnemyTurnStart);
        EventManager.StartListening(EventNames.ENEMY_TURN_COMPLETED, HandleEnemyTurnEnd);
    }

    private void HandleMouseDownEvent(System.Object data)
    {
        if (data is MouseEventActionProvider)
        {
            var mouseEventData = data as MouseEventActionProvider;
            StartCoroutine(mouseEventData.OnMouseDownAction().Perform());
            //Example: The action provider is a player character. They return a ShowMovementGrid action. This action invokes the GridController to show the movement grid. 
            //they might return a HideMovementGrid action. This invokes the GridController to hide the movement grid

            //Example 2: The action provider is an interactable tile. It checks if movement is available for this square. 
            //It might return an InteractWithTile action which performs the behavior of interacting with the tile

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
        EventManager.StartListening(EventNames.START_MOVE, HandleStartMove);
        EventManager.StartListening(EventNames.END_MOVE, HandleEndMove);
    }

    private void StopListenForMoveEvents()
    {
        EventManager.StopListening(EventNames.START_MOVE, HandleStartMove);
        EventManager.StopListening(EventNames.END_MOVE, HandleEndMove);
    }

    private void ListenForMouseEvents()
    {
        EventManager.StartListening(EventNames.MOUSE_DOWN, HandleMouseDownEvent);
        EventManager.StartListening(EventNames.MOUSE_ENTER, HandleMouseEnterEvent);
    }

    private void StopListenForMouseEvents()
    {
        EventManager.StopListening(EventNames.MOUSE_DOWN, HandleMouseDownEvent);
        EventManager.StopListening(EventNames.MOUSE_ENTER, HandleMouseEnterEvent);
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
