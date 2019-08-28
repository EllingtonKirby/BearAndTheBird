using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnComparer : Comparer<EnemyController>
{

    public override int Compare(EnemyController x, EnemyController y)
    {
        if (x != null && y != null)
        {
            var xMover = x.GetComponent<MoveToClosestPlayerEnemyController>();
            var yMover = y.GetComponent<MoveToClosestPlayerEnemyController>();

            if (xMover != null && yMover != null)
            {
                var xClosest = GetNumberOfMovesFromSpot(x);
                var yClosest = GetNumberOfMovesFromSpot(y);
                
                return xClosest.CompareTo(yClosest);
            }
        }
        return 0;
    }

    private int GetNumberOfMovesFromSpot(EnemyController mover)
    {
        return EnemyPlacementController.instance.GetEnemyPathToNearestCharacter(mover).Count;
    }
}
