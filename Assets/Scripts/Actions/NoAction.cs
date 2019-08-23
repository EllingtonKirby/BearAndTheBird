using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAction : Action, EnemyAction
{
    public IEnumerator Perform()
    {
        yield return null;
    }
}
