using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnComparer : Comparer<EnemyController>
{
    private Dictionary<GridTile, int> lengthOfEnemyMoves = new Dictionary<GridTile, int>();


    public void Init()
    {
        lengthOfEnemyMoves.Clear();
    }

    public override int Compare(EnemyController x, EnemyController y)
    {
        if (x != null && y != null)
        {
            var xMover = x.GetComponent<MoveToClosestPlayerEnemyController>();
            var yMover = y.GetComponent<MoveToClosestPlayerEnemyController>();

            if (xMover != null && yMover != null)
            {
                var xClosest = GetNumberOfMovesFromSpot(xMover);
                var yClosest = GetNumberOfMovesFromSpot(yMover);
                
                return xClosest.CompareTo(yClosest);
            }
        }
        return 0;
    }

    private int GetNumberOfMovesFromSpot(MoveToClosestPlayerEnemyController mover)
    {
        var tile = GridController.instance.GetTileAtPosition(mover.transform.position);
        if (!lengthOfEnemyMoves.ContainsKey(tile))
        {
            var closestPlayer = GridController.instance.GetTileAtPosition(mover.FindClosestPlayer().transform.position);
            lengthOfEnemyMoves[tile] = AStarHelper.GetPath(tile, closestPlayer, true).Count;
        }
        return lengthOfEnemyMoves[tile];
    }
}
