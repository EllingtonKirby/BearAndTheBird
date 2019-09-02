using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions = GridController.NeighboringGrids;

[CreateAssetMenu(fileName = "Movement Style", menuName = "Movement Style")]
public class MovementStyle: ScriptableObject
{
    public TypesOfMovement type;
    public bool activatesTraps;

    public int actionCost;
    public int individualMoveCost;
    public int regionFilter;
    public float timeToMove;

    public GameObject movementAvailableTile;
    public List<GridTile.MovementState> elligibleStartingStates;
    public GridTile.MovementState targetState;

    public int moveOffsetA;
    public int moveOffsetB;
}
