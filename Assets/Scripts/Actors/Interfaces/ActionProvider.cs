using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyActionProvider
{
    EnemyAction GetAction();
}

public interface EnemyAction
{
    IEnumerator Perform();
}
