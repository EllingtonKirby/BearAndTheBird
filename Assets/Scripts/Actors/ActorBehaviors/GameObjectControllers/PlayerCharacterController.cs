using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour, MouseEventActionProvider, MovementStyleProvider, StatefulActor
{
    public Vector3 startPos;
    public MovementStyle movementStyle;

    private State state;
    private bool dirtyState = false;

    // Use this for initialization
    void Start()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        state = State.IDLE;
        GridController.instance.MarkGridTileOccupied(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        Debug.Log("Player mouse down event");
        EventManager.TriggerMouseEvent(EventNames.MOUSE_DOWN, this);
    }

    private void OnMouseEnter()
    {
        EventManager.TriggerMouseEvent(EventNames.MOUSE_ENTER, this);
    }

    public Action OnMouseDownAction()
    {
        if (IsIdle())
        {
            if (BreadCrumbController.instance.IsPotentialMoveInProgress() == false)
            {
                if (!ShowMovementAvailableController.instance.CheckIsMoveAvailableFromTile(transform.position))
                {
                    Debug.Log("No movement available");
                    return new ShowNoMovementAvalableAction(transform.position, GetInstanceID());
                }
                else
                {
                    //Easy to abstract this action to do different things depending on the animal type
                    return new ShowMovementAvailableAction(movementStyle, transform.position, this, this.gameObject);
                }
            }
            else
            {
                return new NoAction();
            }
        }
        else
        {
            return new HideMovementAvailableAction(transform.position, this);
        }
    }

    public Action OnMouseEnterAction()
    {
        if (IsSelectedForMovement())
        {
            //Sanity check we shouldn't need these checks
            if (BreadCrumbController.instance.IsPotentialMoveInProgress()
            && BreadCrumbController.instance.GetOriginOfMovement() == transform.position)
            {
                return new ClearBreadCrumbsAction();
            } else
            {
                return new NoAction();
            }
        } else
        {
            return new NoAction();
        }
    }

    public State GetState()
    {
        return state;
    }

    public MovementStyle GetMovementStyle()
    {
        return movementStyle;
    }

    public bool IsIdle()
    {
        return state == State.IDLE;
    }

    public bool IsSelectedForMovement()
    {
        return state == State.SELECTED_FOR_MOVEMENT;
    } 

    public bool IsMoveInProgress()
    {
        return state == State.MOVE_IN_PROGRESS;
    }

    public void SetIdle()
    {
        this.state = State.IDLE;
    }

    public void SetSelectedForMovement()
    {
        this.state = State.SELECTED_FOR_MOVEMENT;
    }

    public void SetMoveInProgress()
    {
        this.state = State.MOVE_IN_PROGRESS;
    }
}
