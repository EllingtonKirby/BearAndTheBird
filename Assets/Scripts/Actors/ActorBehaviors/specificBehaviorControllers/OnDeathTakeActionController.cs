using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnDeathTakeActionController : MonoBehaviour
{
    public abstract EnemyAction OnDeath();
}
