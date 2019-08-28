
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderMineController : EnemyController
{
    public int visionRange;

    MoveToClosestPlayerEnemyController movementController;
    OnExplodeController explosionController;

    //Logic to determine this enemies AI will go here
    //Need to figure out how to notify the OnExplodeController from the OnPlayerAdjacentChangeSpriteController

    void Start()
    {
        movementController = GetComponent<MoveToClosestPlayerEnemyController>();
        explosionController = GetComponent<OnExplodeController>();
    }

    void Update()
    {

    }

    public override EnemyAction GetAction()
    {
        if (explosionController.IsPrimedForExplosion())
        {
            return explosionController.GetAction();
        } else
        {
            var pathToNearest = EnemyPlacementController.instance.GetEnemyPathToNearestCharacter(this);
            if (pathToNearest.Count <= visionRange)
            {
                return movementController.GetAction();
            } else
            {
                return new NoTurnAction();
            }
        }
    }
}
