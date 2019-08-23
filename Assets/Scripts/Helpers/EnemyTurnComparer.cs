using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnComparer : Comparer<EnemyController>
{
    public override int Compare(EnemyController x, EnemyController y)
    {
        var xMover = x.GetComponent<MoveToClosestPlayerEnemyController>();
        var yMover = y.GetComponent<MoveToClosestPlayerEnemyController>();
        if (xMover != null && yMover != null)
        {
            var xClosest = xMover.FindClosestUnoccupiedTile(xMover.FindClosestPlayer());
            var distanceX = Vector3.Distance(x.transform.position, xClosest.WorldLocation);
            var yClosest = yMover.FindClosestUnoccupiedTile(yMover.FindClosestPlayer());
            var distanceY = Vector3.Distance(y.transform.position, yClosest.WorldLocation);
            return distanceX.CompareTo(distanceY);
        } else
        {
            return 0;
        }
    }
}
