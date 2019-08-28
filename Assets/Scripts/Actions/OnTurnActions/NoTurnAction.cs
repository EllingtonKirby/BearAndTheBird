using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTurnAction : Action, EnemyAction
{
    public IEnumerator Perform()
    {
        EventManager.TriggerEvent(EventNames.END_MOVE);
        yield return new WaitForEndOfFrame();
    }
}
