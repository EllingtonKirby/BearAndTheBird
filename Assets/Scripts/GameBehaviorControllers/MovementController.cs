using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{

    public static MovementController instance;

    private List<Vector2> pointsToMoveTo;
    private StatefulActor charSelected;
    private GameObject queuedToMove;
    private float timeToMove;
    private Coroutine currentMove;
    private readonly object coroutineLock = new object();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        pointsToMoveTo = new List<Vector2>();
        EventManager.StartListening(EventNames.MOVEMENT_QUEUED, OnMovementAddedToQueue);
        EventManager.StartListening(EventNames.MOVEMENT_REMOVED_FROM_QUEUE, OnMovementRemovedFromQueue);
        EventManager.StartListening(EventNames.TERMINATE_MOVE, TerminateMove);
    }

    private void OnMovementAddedToQueue(System.Object data)
    {
        if (data is Vector2?)
        {
            var newPosition = data as Vector2?;
            var newPos = newPosition ?? default(Vector2);
            pointsToMoveTo.Add(newPos);
        }
    }

    private void OnMovementRemovedFromQueue(System.Object data)
    {
        if (data is Vector2)
        {
            var newPosition = data as Vector2?;
            var newPos = newPosition ?? default(Vector2);
            pointsToMoveTo.Remove(newPos);
        }
    }

    private void TerminateMove(System.Object data)
    {
        lock (coroutineLock)
        {
            if (currentMove != null)
            {
                var copyOfPointsToMove = new List<Vector2>(pointsToMoveTo);               
                StopAllCoroutines();
                if (copyOfPointsToMove.Count > 0)
                {
                    if (queuedToMove.tag == "Player")
                    {
                        foreach (Vector2 position in copyOfPointsToMove)
                        {
                            var eventName = string.Format(EventNames.MOVED_TO_POSITION, position.x, position.y);
                            Debug.Log(eventName);
                            EventManager.TriggerEvent(eventName);
                        }
                    }
                    pointsToMoveTo.Clear();
                }
                if (charSelected != null)
                {
                    charSelected.SetIdle();
                }
                CancelPreparedMovement();
                currentMove = null;
                EventManager.TriggerEvent(EventNames.END_MOVE);
            }
        }
    }

    public void CancelPreparedMovement()
    {
        queuedToMove = null;
        charSelected = null;
        timeToMove = float.MinValue;
    }

    public void PrepareMovementForObject(GameObject toMove, StatefulActor charSelected, float timeToMove)
    {
        queuedToMove = toMove;
        this.charSelected = charSelected;
        this.timeToMove = timeToMove;
    }

    public IEnumerator ConsumeMovementQueueForObject()
    {
        if (queuedToMove == null)
        {
            Debug.Log("Trying to trigger a move without a queued object");
            yield return null;
        }
        if (System.Math.Abs(timeToMove - float.MinValue) < Mathf.Epsilon)
        {
            Debug.Log("Trying to trigger a move without a time to move");
            yield return null;
        }
        currentMove = StartCoroutine(MoveToPosition(queuedToMove.transform, timeToMove));
        yield return currentMove;
    }

    public IEnumerator MoveToPosition(Transform transform, float timeToMove)
    {
        for (int i = 0; i < pointsToMoveTo.Count; i++)
        {
            GridController.instance.MarkGridTileUnOccupied(transform.position);
            Vector2 grid = pointsToMoveTo[i];
            Vector3 position = new Vector3(grid.x, grid.y, transform.position.z);

            yield return StartCoroutine(PerformMovement(transform, position));
        }

        pointsToMoveTo.Clear();
        charSelected.SetIdle();
        currentMove = null;
        EventManager.TriggerEvent(EventNames.END_MOVE);
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator PerformMovement(Transform toMove, Vector3 position)
    {
        while (System.Math.Abs(Vector3.Distance(toMove.position, position)) > Mathf.Epsilon)
        {
            var t = Time.deltaTime * timeToMove;
            toMove.position = Vector3.MoveTowards(toMove.position, position, t);
            yield return null;
        }

        GridController.instance.MarkGridTileOccupied(toMove.position);
        var eventName = string.Format(EventNames.MOVED_TO_POSITION, position.x, position.y);
        EventManager.TriggerEvent(eventName);
        yield return new WaitForEndOfFrame();
    }
}
