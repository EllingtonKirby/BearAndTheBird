using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMovementAvailableAction : Action
{
    private readonly Vector2 startPosition;
    private readonly PlayerCharacterController charSelected;

    public HideMovementAvailableAction(Vector2 startPosition, PlayerCharacterController charSelected)
    {
        this.startPosition = startPosition;
        this.charSelected = charSelected;
    }

    public IEnumerator Perform()
    {
        Debug.Log("Hiding movement available");
        charSelected.SetIdle();
        //Order matters here, need to clear existing bread crumb trail
        MovementController.instance.CancelPreparedMovement();
        BreadCrumbController.instance.CancelMovement();
        ShowMovementAvailableController.instance.HideSquaresElligibleForMoveTile();
        yield return null;
    }
}
