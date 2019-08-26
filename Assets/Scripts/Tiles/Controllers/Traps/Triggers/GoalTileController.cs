using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTileController : TrapController
{
    public override void OnEnemyEnter(EnemyHealthController enemyHealth)
    {
        enemyHealth.ReachGoal();
    }

    public override void OnPlayerEnter(PlayerHealthController playerHealth)
    {
        playerHealth.ReachGoal();
    }
}
