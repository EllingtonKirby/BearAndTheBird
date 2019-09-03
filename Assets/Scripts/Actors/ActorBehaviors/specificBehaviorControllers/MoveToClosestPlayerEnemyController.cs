using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions = GridController.NeighboringGrids;

public class MoveToClosestPlayerEnemyController : MonoBehaviour, EnemyActionProvider, StatefulActor
{
    public MovementStyle movementStyle;
    public int availableMove;

    private State state;
    private EnemyController enemyController;

    //This is always called after the objects have been sorted, use cached position
    public EnemyAction GetAction()
    {
        var positionOfNearestCharacter = FindClosestPlayer().transform.position;

        //Need to recalculate path as may have changed by previous moves
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

    public PlayerCharacterController FindClosestPlayer()
    {
        return EnemyPlacementController.instance.GetNearestCharacter(enemyController);
    }


    private Vector2 GetWorldPositionOfGridTile(GridTile tile)
    {
        return tile.WorldLocation;
    }

    void Start()
    {
        GridController.instance.MarkGridTileOccupied(transform.position);
        enemyController = GetComponentInParent<EnemyController>();
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
