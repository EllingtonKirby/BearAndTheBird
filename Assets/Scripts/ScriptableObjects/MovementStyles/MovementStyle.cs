using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Style", menuName = "Movement Style")]
public class MovementStyle: ScriptableObject
{
    public bool activatesTraps;
    public int actionCost;
    public float timeToMove;
    public GameObject movementAvailableTile;
}
