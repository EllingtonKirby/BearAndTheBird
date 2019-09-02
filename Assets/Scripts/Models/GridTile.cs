using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTile 
{
    public Vector3Int LocalPlace { get; set; }

    public Vector3 WorldLocation { get; set; }

    public TileBase TileBase { get; set; }

    public Tilemap TilemapMember { get; set; }

    public MovementState State { get; set; }

    public string Name { get; set; }

    public int Cost { get; set; }

    public int CurrentMovementValue{ get; set; }

    private MovementState previousState;

    public void UpdateState(MovementState newState)
    {
        previousState = State;
        State = newState;
    }

    public void ResetToLastState()
    {
        State = previousState;
    }

    public enum MovementState
    {
        DEFAULT,
        ELLIGIBLE_FOR_MOVE,
        QUEUED_FOR_MOVE,
        OCCUPIED,
        COLLIDER,
        NOT_IN_GRID
    }
}
