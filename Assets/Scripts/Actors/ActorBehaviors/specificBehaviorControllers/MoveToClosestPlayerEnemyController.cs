using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions = GridController.NeighboringGrids;

public class MoveToClosestPlayerEnemyController : MonoBehaviour, EnemyActionProvider, StatefulActor
{
    public MovementStyle movementStyle;
    public int availableMove;
    private State state;

    public EnemyAction GetAction()
    {
        var positionOfNearestCharacter = FindClosestPlayer().transform.position;
        var pathToNearestCharacter = AStarHelper.GetPath(
            GridController.instance.GetTileAtPosition(transform.position),
            GridController.instance.GetTileAtPosition(positionOfNearestCharacter)
        );
        var list = pathToNearestCharacter.ConvertAll(
            new System.Converter<GridTile, Vector2>(GetWorldPositionOfGridTile)
        );
        //Need to find the nearest character and create an int list of positions to move towards
        if (list.Count > 0)
        {
            return new MoveToTargetPositionAction(
                gameObject,
                this,
                movementStyle,
                list.GetRange(0, Mathf.Min(availableMove - 1, list.Count))
            );
        } else
        {
            return new MoveToTargetPositionAction(
             gameObject,
             this,
             movementStyle,
             list
         );
        }
    }

    public GameObject FindClosestPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    public GridTile FindClosestUnoccupiedTile(GameObject player)
    {
        //Flood fill via occupied state to find closest unoccupied tile
        //  We probably want to do these flood fill operations pretty often
        //Then A* towards that tile by enemies movement speed
        var queue = new Queue<GridTile>();
        queue.Enqueue(GridController.instance.GetTileAtPosition(player.transform.position));

        while (queue.Count > 0)
        {
            var top = queue.Peek();
            queue.Dequeue();
            if (top == null)
            {
                continue;
            }
            if (top.State != GridTile.MovementState.OCCUPIED)
            {
                return top;
            }
            else
            {
                foreach (Directions direction in System.Enum.GetValues(typeof(Directions)))
                {
                    queue.Enqueue(GridController.instance.GetNeighborAt(direction, top.WorldLocation));
                }
            }
        }

        return null;
    }

    private Vector2 GetWorldPositionOfGridTile(GridTile tile)
    {
        return tile.WorldLocation;
    }

    void Start()
    {
        GridController.instance.MarkGridTileOccupied(transform.position);
    }

    public void SetSelectedForMovement()
    {
        state = State.SELECTED_FOR_MOVEMENT;
    }

    public void SetIdle()
    {
        state = State.IDLE;
    }

    public State GetState()
    {
        return state;
    }
}
