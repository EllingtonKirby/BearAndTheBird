using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBreadCrumbAction : Action
{
    private readonly GroundTileByTileMovementAvailableController gridSquareController;
    private readonly Vector2 positionToAdd;
    private readonly int movementCostOfSquare;

    public PopBreadCrumbAction(GroundTileByTileMovementAvailableController gridSquareController, Vector2 positionToAdd, int movementCostOfSquare)
    {
        this.gridSquareController = gridSquareController;
        this.positionToAdd = positionToAdd;
        this.movementCostOfSquare = movementCostOfSquare;
    }

    public IEnumerator Perform()
    {
        BreadCrumbController.instance.PopToCurrentPosition(new BreadCrumb(positionToAdd, movementCostOfSquare));
        yield return null;
    }
}
