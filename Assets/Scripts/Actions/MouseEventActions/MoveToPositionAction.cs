using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPositionAction : Action
{
    public MoveToPositionAction()
    {
    }

    public IEnumerator Perform()
    {
        EventManager.TriggerEvent(EventNames.START_MOVE);
        ShowMovementAvailableController.instance.HideSquaresElligibleForMoveTile();
        BreadCrumbController.instance.ClearOriginOfMovement();
        BreadCrumbController.instance.ClearMovementStack();
        yield return MovementController.instance.ConsumeMovementQueueForObject();
    }
}
