using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnPlayerAdjacentActionController : MonoBehaviour, AdjacentPlayerActionProvider
{
    /**
     *  Ideas
     *      This class should be abstract, and we will have different actions that can occur when 
     *      when a player is adjacent subclass this 
     *      They will invoke their own Action to perform when the player is adjacent and take
     *      their own parameters etc
     *      
     *      We want this to disable under certain circumstances, seems like END_MOVE makes the most sense. 
     *      
     **/
    private void Start()
    {
        EventManager.StartListening(EventNames.END_MOVE, OnEndMoveEvent);
    }

    private void OnEndMoveEvent(object arg0)
    {
        //Determine if the player is in one of the 4 adjacent squares
        //Trigger the action if so

        //Raycast to the four adjacent squares
        var position = new Vector2(transform.position.x, transform.position.y);
        var positionNorth = position + Vector2.up;
        var positionEast = position + Vector2.right;
        var positionSouth = position + Vector2.down;
        var positionWest = position + Vector2.left;

        Collider2D hitNorth = Physics2D.OverlapCircle(positionNorth, .25f);
        Collider2D hitEast = Physics2D.OverlapCircle(positionEast, .25f);
        Collider2D hitSouth = Physics2D.OverlapCircle(positionSouth, .25f);
        Collider2D hitWest = Physics2D.OverlapCircle(positionWest, .25f);

        var listOfHits = new ArrayList() { hitNorth, hitEast, hitSouth, hitWest };
        foreach (Collider2D hit in listOfHits)
        {
            if (hit != null && hit.transform.gameObject.tag == "Player")
            {
                var action = GetOnPlayerAdjacentAction(hit.transform.position);
                if (TurnController.instance.GetState() == TurnController.TurnState.ENEMY_TURN)
                {
                    ActionResolutionQueueController.instance.EnqueueEnemyAction(action);
                } else
                {
                    StartCoroutine(action.Perform());
                }
            }
        }
    }

    public abstract OnAdjacentPlayerAction GetOnPlayerAdjacentAction(Vector3 position);
}
