using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetPositionAction : EnemyAction
{
    private readonly GameObject toMove;
    private readonly StatefulActor charSelected;
    private readonly MovementStyle movementStyle;
    private readonly List<Vector2> positionsToMoveTo;

    public MoveToTargetPositionAction(GameObject toMove, StatefulActor charSelected, MovementStyle movementStyle, List<Vector2> positionsToMoveTo)
    {
        this.toMove = toMove;
        this.charSelected = charSelected;
        this.movementStyle = movementStyle;
        this.positionsToMoveTo = positionsToMoveTo;
    }

    public IEnumerator Perform()
    {
        charSelected.SetSelectedForMovement();
        MovementController.instance.PrepareMovementForObject(toMove, charSelected, movementStyle.timeToMove);
        foreach(Vector2 position in positionsToMoveTo)
        {
            EventManager.TriggerEvent(EventNames.MOVEMENT_QUEUED, position);
        }
        yield return MovementController.instance.ConsumeMovementQueueForObject();
    }
}
