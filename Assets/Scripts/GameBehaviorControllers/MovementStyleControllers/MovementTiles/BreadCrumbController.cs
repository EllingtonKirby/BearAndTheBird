using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCrumbController : MonoBehaviour
{
    public static BreadCrumbController instance;

    public GameObject breadCrumbPrefab;

    private Stack<BreadCrumb> movementStack = new Stack<BreadCrumb>();
    private Stack<GameObject> breadCrumbs = new Stack<GameObject>();

    private Vector3? originOfMovement;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

    }

    private void Start()
    {
        originOfMovement = null;
    }

    public void InitializeBreadCrumbTrail(Vector3 origin)
    {
        originOfMovement = origin;
    }

    public bool IsPotentialMoveInProgress()
    {
        return originOfMovement != null;
    }

    public void ClearOriginOfMovement()
    {
        originOfMovement = null;
    }

    public Vector2? GetOriginOfMovement()
    {
        return originOfMovement;
    }

    public bool AddPointToStack(BreadCrumb toAdd)
    {
        if (movementStack.Count == 0 && IsMoveAvailable(toAdd.movementCost))
        {
            if (Vector2.Distance(originOfMovement.Value, toAdd.position) > 1f)
            {
                return false;
            }
            CreateBreadCrumb(toAdd);
            return true;
        }
        else if (!IsMoveAvailable(toAdd.movementCost))
        {
            return false;
        }
        BreadCrumb lastItem = movementStack.Peek();
        float distance = Vector2.Distance(toAdd.position, lastItem.position);
        if (distance == 1f && !movementStack.Contains(toAdd))
        {
            if (IsMoveAvailable(toAdd.movementCost))
            {
                CreateBreadCrumb(toAdd);
                return true;
            }
        }
        return false;
    }

    public void MoveSingleBreadCrumb(BreadCrumb toAdd)
    {
        if (movementStack.Count > 0)
        {
            Debug.Log("Deleting first breadcrumb");
            CheckAndPopFirstMove();
        }
        if (IsMoveAvailable(toAdd.movementCost))
        {
            CreateBreadCrumb(toAdd);
        } 
    }

    public void PopToCurrentPosition(BreadCrumb currentPosition)
    {
        if (movementStack.Count == 0)
        {
            Debug.Log("Popping out of an empty stack, shouldn't be doing this");
            return;
        }
        while (!movementStack.Peek().Equals(currentPosition))
        {
            DestroyBreadCrumb(movementStack.Peek());
        }
    }

    public void CheckAndPopFirstMove()
    {
        if (movementStack.Count == 1)
        {
            DestroyBreadCrumb(movementStack.Peek());
        }
    }

    public void CancelMovement()
    {
        while (movementStack.Count > 0)
        {
            DestroyBreadCrumb(movementStack.Peek());
        }
        ClearOriginOfMovement();
    }

    public void ClearMovementStack()
    {
        movementStack.Clear();
    }

    private bool IsMoveAvailable(int costOfMove)
    {
        if (!TurnController.instance.CheckCostOfMoveAgainstActionPointsRemaining(costOfMove))
        {
            Debug.Log("Not Enough Action Points");
            return false;
        }
        return true;
    }

    private void UpdateCurrentMovementValues(int costOfMove)
    {
        TurnController.instance.UseActionPoint(costOfMove);
    }

    private void CreateBreadCrumb(BreadCrumb toAdd)
    {
        UpdateCurrentMovementValues(toAdd.movementCost);

        movementStack.Push(toAdd);
        var created = Instantiate(breadCrumbPrefab, toAdd.position, Quaternion.identity);
        breadCrumbs.Push(created);

        var eventName = string.Format(EventNames.ADDED_TO_BREADCRUMB, toAdd.position.x, toAdd.position.y);
        EventManager.TriggerEvent(eventName, toAdd);
        EventManager.TriggerEvent(EventNames.MOVEMENT_QUEUED, toAdd.position);
    }

    private void DestroyBreadCrumb(BreadCrumb toRemove)
    {
        GameObject toPop = breadCrumbs.Peek();
        Destroy(toPop);
        UpdateCurrentMovementValues(toRemove.movementCost* -1);

        breadCrumbs.Pop();
        movementStack.Pop();

        var eventName = string.Format(EventNames.REMOVED_FROM_BREADCRUMB, toRemove.position.x, toRemove.position.y);
        EventManager.TriggerEvent(eventName, toRemove);
        EventManager.TriggerEvent(EventNames.MOVEMENT_REMOVED_FROM_QUEUE, toRemove.position);
    }
}
