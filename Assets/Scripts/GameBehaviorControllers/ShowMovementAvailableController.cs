using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Directions = GridController.NeighboringGrids;

public class ShowMovementAvailableController : MonoBehaviour
{
    public static ShowMovementAvailableController instance;
    private Dictionary<Vector3, GameObject> instancedMovementGrids;
    private GameObject tile;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

    }

    private void Start()
    {
        instancedMovementGrids = new Dictionary<Vector3, GameObject>();
    }

    public void HideSquaresElligibleForMoveTile()
    {
        this.tile = null;
        foreach (Vector3 vector3 in instancedMovementGrids.Keys)
        {
            var neighbor = GridController.instance.GetTileAtPosition(vector3);
            if (neighbor.State == GridTile.MovementState.ELLIGIBLE_FOR_MOVE)
            {
                neighbor.State = GridTile.MovementState.DEFAULT;
            }
            neighbor.CurrentMovementValue = int.MinValue;
            if (instancedMovementGrids[neighbor.WorldLocation] != null)
            {
                Destroy(instancedMovementGrids[neighbor.WorldLocation]);
            }
        }
        instancedMovementGrids.Clear();
    }

    public bool CheckIsMoveAvailableFromTile(Vector3 origin)
    {
        var top = GridController.instance.GetTileAtPosition(origin);
        top.CurrentMovementValue = TurnController.instance.GetActionPointsRemaining();
        foreach (Directions direction in System.Enum.GetValues(typeof(Directions)))
        {
            var neighbor = GridController.instance.GetNeighborAt(direction, top.WorldLocation);
            if (neighbor != null && neighbor.State == GridTile.MovementState.DEFAULT)
            {
                if (top.CurrentMovementValue - neighbor.Cost >= 0 && top.CurrentMovementValue - neighbor.Cost > neighbor.CurrentMovementValue)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void ShowSquaresElligibleForMoveTile(Vector3 origin, int move, GameObject tile)
    {
        this.tile = tile;
        FloodFillTile(GridController.instance.GetTileAtPosition(origin), GridTile.MovementState.DEFAULT, GridTile.MovementState.ELLIGIBLE_FOR_MOVE, move, FloodFillBehavior.SHOW_MOVEMENT_AVAILABLE);
    }

    public void FloodFillTile(GridTile start, GridTile.MovementState targetState, GridTile.MovementState replacementState, int startingMovement, FloodFillBehavior behavior)
    {
        switch (behavior)
        {
            case FloodFillBehavior.SHOW_MOVEMENT_AVAILABLE:
                start.State = GridTile.MovementState.OCCUPIED;
                start.CurrentMovementValue = startingMovement;
                instancedMovementGrids[start.WorldLocation] = null;
                break;
            case FloodFillBehavior.HIDE_MOVEMENT_AVAILABLE:
                return;
        }

        Queue<GridTile> queue = new Queue<GridTile>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var top = queue.Peek();
            queue.Dequeue();

            foreach (Directions direction in System.Enum.GetValues(typeof(Directions)))
            {
                switch (behavior)
                {
                    case FloodFillBehavior.SHOW_MOVEMENT_AVAILABLE:
                        CheckThenSetStateOfNeighborAtTile(top, direction, targetState, replacementState, queue, top.CurrentMovementValue);
                        break;
                    case FloodFillBehavior.HIDE_MOVEMENT_AVAILABLE:
                        break;
                }
            }
        }
    }

    private void CheckThenSetStateOfNeighborAtTile(GridTile top, Directions direction, GridTile.MovementState targetState, GridTile.MovementState replacementState, Queue<GridTile> q, int movementRemaining)
    {
        var neighbor = GridController.instance.GetNeighborAt(direction, top.WorldLocation);
        if (neighbor != null && neighbor.State == targetState)
        {
            if (top.CurrentMovementValue - neighbor.Cost >= 0 && top.CurrentMovementValue - neighbor.Cost > neighbor.CurrentMovementValue)
            {
                neighbor.State = replacementState;
                neighbor.CurrentMovementValue = top.CurrentMovementValue - neighbor.Cost;
                q.Enqueue(neighbor);

                var selectedGrid = Instantiate(tile, neighbor.WorldLocation, Quaternion.identity);
                instancedMovementGrids.Add(neighbor.WorldLocation, selectedGrid);
            }
        }
    }
}
