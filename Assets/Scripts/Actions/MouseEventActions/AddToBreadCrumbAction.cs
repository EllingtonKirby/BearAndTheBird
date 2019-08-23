using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToBreadCrumbAction : Action
{
    private readonly GroundTileByTileMovementAvailableController gridSquareController;
    private readonly Vector2 positionToAdd;
    private readonly int movementCostOfSquare;

    public AddToBreadCrumbAction(GroundTileByTileMovementAvailableController gridSquareController, Vector2 positionToAdd, int movementCostOfSquare)
    {
        this.gridSquareController = gridSquareController;
        this.positionToAdd = positionToAdd;
        this.movementCostOfSquare = movementCostOfSquare;
    }

    public IEnumerator Perform()
    {
        BreadCrumbController.instance.AddPointToStack(new BreadCrumb(positionToAdd, movementCostOfSquare));
        yield return null;
    }
}
