using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AdjacentPlayerActionProvider 
{
    OnAdjacentPlayerAction GetOnPlayerAdjacentAction(Vector3 position);
}

public interface OnAdjacentPlayerAction : EnemyAction { }
