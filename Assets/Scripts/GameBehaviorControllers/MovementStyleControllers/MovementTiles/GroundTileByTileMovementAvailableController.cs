using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTileByTileMovementAvailableController : MonoBehaviour, MouseEventActionProvider
{
    private State state = State.ELLIGIBLE_FOR_MOVE;
    private int movementCost;
    private bool dirtyState = false;


    void Start()
    {
        var eventNameAdded = string.Format(EventNames.ADDED_TO_BREADCRUMB, transform.position.x, transform.position.y);
        EventManager.StartListening(eventNameAdded, OnAddedToBreadCrumb);

        var eventNameRemoved = string.Format(EventNames.REMOVED_FROM_BREADCRUMB, transform.position.x, transform.position.y);
        EventManager.StartListening(eventNameRemoved, OnRemovedFromBreadCrumb);

        movementCost = GridController.instance.GetTileAtPosition(transform.position).Cost;
    }

    void Update()
    {
        if (dirtyState)
        {
            switch (state)
            {
                case State.ELLIGIBLE_FOR_MOVE:
                    break;
                case State.QUEUED_FOR_MOVE:
                    break;
                case State.OCCUPIED:
                    break;
            }
            dirtyState = false;
        }
    }

    public Action OnMouseDownAction()
    {
        return new NoAction();
    }

    public Action OnMouseEnterAction()
    {
        if (state == State.ELLIGIBLE_FOR_MOVE)
        {
            return new AddToBreadCrumbAction(this, this.transform.position, movementCost);
        }
        else if (state == State.QUEUED_FOR_MOVE)
        {
            return new PopBreadCrumbAction(this, this.transform.position, movementCost);
        }
        else
        {
            return new NoAction();
        }
    }

    public void OnAddedToBreadCrumb(System.Object data)
    {
        this.state = State.QUEUED_FOR_MOVE;
        dirtyState = true;
    }

    public void OnRemovedFromBreadCrumb(System.Object data)
    {
        this.state = State.ELLIGIBLE_FOR_MOVE;
        dirtyState = true;
    }

    public State GetState()
    {
        return state;
    }

    public void SetState(State newState)
    {
        state = newState;
        dirtyState = true;
    }

    public enum State
    {
        ELLIGIBLE_FOR_MOVE,
        QUEUED_FOR_MOVE,
        OCCUPIED
    }
}
