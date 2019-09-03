using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathExplodeController : OnDeathTakeActionController
{

    public Explosions explosion;

    public override EnemyAction OnDeath()
    {
        return new OnTurnExplodeAction(explosion, transform, false);
    }
}
