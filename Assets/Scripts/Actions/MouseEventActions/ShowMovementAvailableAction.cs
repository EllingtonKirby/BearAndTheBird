using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMovementAvailableAction : Action
{
    private readonly MovementStyle movementStyle;
    private readonly Vector2 startPosition;
    private readonly StatefulActor charSelected;
    private readonly GameObject toMove;
    private readonly List<GridTile> tiles;

    public ShowMovementAvailableAction(MovementStyle movementStyle, Vector3 startPosition, StatefulActor charSelected, GameObject toMove, List<GridTile> tiles)
    {
        this.movementStyle = movementStyle;
        this.startPosition = startPosition;
        this.charSelected = charSelected;
        this.toMove = toMove;
        this.tiles = tiles;
    }

    public IEnumerator Perform()
    {
        //var currentActionPointsAvailable = TurnController.instance.GetActionPointsRemaining();
        //if (currentActionPointsAvailable > 0)
        //{
            charSelected.SetSelectedForMovement();
            MovementController.instance.PrepareMovementForObject(toMove, charSelected, movementStyle.timeToMove);
            BreadCrumbController.instance.InitializeBreadCrumbTrail(startPosition);
            ShowMovementAvailableController.instance.ShowMovementAvailable(tiles, movementStyle.movementAvailableTile);
        //}
        yield return null;
    }
}
