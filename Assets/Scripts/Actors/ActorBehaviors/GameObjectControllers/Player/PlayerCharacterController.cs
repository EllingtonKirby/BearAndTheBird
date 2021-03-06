﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour, MouseEventActionProvider, MovementStyleProvider, StatefulActor
{
    public MovementStyle movementStyle;
    public CharacterTypes type;

    private State state;
    private bool dirtyState = false;

    // Use this for initialization
    void Start()
    {
        state = State.IDLE;
        GridController.instance.MarkGridTileOccupied(transform.position);
    }

    public Action OnMouseDownAction()
    {
        if (IsIdle())
        {
            if (BreadCrumbController.instance.IsPotentialMoveInProgress() == false)
            {
                var start = GridController.instance.GetTileAtPosition(transform.position);
                var tiles = ShowMovementAvailableController.instance.GetElligibleMovesFromPosition(start, movementStyle);
                if (tiles.Count == 0)
                {
                    Debug.Log("No movement available");
                    return new ShowNoMovementAvalableAction(transform.position, GetInstanceID());
                }
                else
                {
                    return new ShowMovementAvailableAction(movementStyle, transform.position, this, this.gameObject, tiles);
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
