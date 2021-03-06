﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Directions = GridController.NeighboringGrids;

public class ShowMovementAvailableController : MonoBehaviour
{
    public static ShowMovementAvailableController instance;
    private Dictionary<Vector3, GameObject> instancedMovementGrids;
    private List<GridTile> tilesToReset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

    }

    private void Start()
    {
        instancedMovementGrids = new Dictionary<Vector3, GameObject>();
        tilesToReset = new List<GridTile>();
    }

    public List<GridTile> GetElligibleMovesFromPosition(GridTile start, MovementStyle style)
    {
        switch (style.type)
        {
            case TypesOfMovement.FLYING_BY_STRAIGHT_UNTIL_BARRIER:
                return GetMovesInDirectionUntilBarrier(start, style);
            case TypesOfMovement.FLYING_BY_REGION:
            case TypesOfMovement.GROUND_BY_FLOOD_FILL:
                return FloodFillTile(start, style);
            case TypesOfMovement.GROUND_BY_HOP:
                return GetTilesForShapeMove(start, style);
            case TypesOfMovement.GROUND_BY_SET_CARDINAL_DIRECTION:
                return GetTilesInCardinalDirections(start, style);
            case TypesOfMovement.GROUND_BY_CHARGE_ATTACK:
                return GetValidTilesInChargeDirection(start, style);
            default:
                return new List<GridTile>();
        }

    }

    public void ShowMovementAvailable(List<GridTile> tiles, GameObject toShow)
    {
        foreach(GridTile location in tiles)
        {
            var selectedGrid = Instantiate(toShow, location.WorldLocation, Quaternion.identity);
            instancedMovementGrids.Add(location.WorldLocation, selectedGrid);
        }
    }

    public void HideMovementAvailable()
    {
        foreach(GridTile position in tilesToReset)
        {
            position.CurrentMovementValue = int.MinValue;
            position.ResetToLastState();
        }

        tilesToReset.Clear();

        foreach (Vector3 vector3 in instancedMovementGrids.Keys)
        {
            var neighbor = GridController.instance.GetTileAtPosition(vector3);
            neighbor.ResetToLastState();
            neighbor.CurrentMovementValue = int.MinValue;
            if (instancedMovementGrids[neighbor.WorldLocation] != null)
            {
                Destroy(instancedMovementGrids[neighbor.WorldLocation]);
            }
        }
        instancedMovementGrids.Clear();
    }

    #region Flood Fills

    public List<GridTile> FloodFillTile(GridTile start, MovementStyle style)
    {
        var toInstantiate = new List<GridTile>();

        start.CurrentMovementValue = style.actionCost;
        tilesToReset.Add(start);

        Queue<GridTile> queue = new Queue<GridTile>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var top = queue.Peek();
            queue.Dequeue();

            foreach (Directions direction in System.Enum.GetValues(typeof(Directions)))
            {
                GridTile shouldAdd;
                if (style.type == TypesOfMovement.FLYING_BY_REGION)
                {
                    shouldAdd = CheckIfTileInRegion(top, direction, queue, style);
                }
                else if (style.type == TypesOfMovement.GROUND_BY_CHARGE_ATTACK)
                {
                    //We need to find if a tile is elligble for a charge attack. The tile has to be occupied by an enemy and within an amount of moves = action points + 1
                    //If the tile is elligible for a charge attack it becomes the last elligible tile for a move in that direction. 
                    shouldAdd = null;
                }
                else
                {
                    shouldAdd = CheckThenSetStateOfNeighborAtTile(top, direction, queue, style);
                }
                if (shouldAdd != null)
                {
                    toInstantiate.Add(shouldAdd);
                }
            }
        }

        return toInstantiate;
    }

    private GridTile CheckThenSetStateOfNeighborAtTile(GridTile top, Directions direction, Queue<GridTile> q, MovementStyle style)
    {
        var neighbor = GridController.instance.GetNeighborAt(direction, top.WorldLocation);
        if (neighbor != null && style.elligibleStartingStates.Contains(neighbor.State))
        {
            if (top.CurrentMovementValue - style.individualMoveCost >= 0 && top.CurrentMovementValue - style.individualMoveCost > neighbor.CurrentMovementValue)
            {
                neighbor.UpdateState(style.targetState);
                neighbor.CurrentMovementValue = top.CurrentMovementValue - neighbor.Cost;
                q.Enqueue(neighbor);

                return neighbor;
            }
        }
        return null;
    }


    #endregion

    #region Region Fills

    private GridTile CheckIfTileInRegion(GridTile top, Directions direction, Queue<GridTile> q, MovementStyle style)
    {
        var neighbor = GridController.instance.GetNeighborAt(direction, top.WorldLocation);
        if (neighbor != null)
        {
            var moveNormallyEligible = top.CurrentMovementValue - style.individualMoveCost >= 0 && top.CurrentMovementValue - style.individualMoveCost > neighbor.CurrentMovementValue;
            var moveInExcludedRegion = moveNormallyEligible && (top.CurrentMovementValue - style.individualMoveCost > style.actionCost - style.regionFilter);
            if (moveNormallyEligible)
            {
                neighbor.CurrentMovementValue = top.CurrentMovementValue - neighbor.Cost;
                q.Enqueue(neighbor);

                if (!moveInExcludedRegion && style.elligibleStartingStates.Contains(neighbor.State))
                {
                    neighbor.UpdateState(style.targetState);
                    return neighbor;
                }
                else
                {
                    neighbor.UpdateState(style.targetState);
                    tilesToReset.Add(neighbor);
                }
            }
        }
        return null;
    }

    #endregion

    #region Shape Moves

    public List<GridTile> GetTilesForShapeMove(GridTile start, MovementStyle movementStyle)
    {
        var tiles = new List<GridTile>();
        var placesToCheck = GetTilesToCheckForShapeMove(start, movementStyle);
        foreach(Vector3 position in placesToCheck)
        {
            var tileAt = GridController.instance.GetTileAtPosition(position);
            if (tileAt != null)
            {
                if (movementStyle.elligibleStartingStates.Contains(tileAt.State))
                {
                    tileAt.UpdateState(movementStyle.targetState);
                    tiles.Add(tileAt);
                }
            }
        }

        return tiles;
    }

    private List<Vector3> GetTilesToCheckForShapeMove(GridTile start, MovementStyle movementStyle)
    {
        var offsetA = movementStyle.moveOffsetA;
        var offsetB = movementStyle.moveOffsetB;

        var startX = start.WorldLocation.x;
        var startY = start.WorldLocation.y;

        var offSets = GetOffsetsFromPosition(offsetA, offsetB);

        var tilesToCheck = new List<Vector3>();

        foreach (KeyValuePair<int, int> offset in offSets)
        {
            tilesToCheck.Add(new Vector3(startX + offset.Key, startY + offset.Value));
        }

        return tilesToCheck;
    }

    private List<KeyValuePair<int, int>> GetOffsetsFromPosition(int offsetA, int offsetB)
    {
        return new List<KeyValuePair<int, int>>
        {
            new KeyValuePair<int, int>(offsetA, offsetB),
            new KeyValuePair<int, int>(-1 * offsetA, offsetB),
            new KeyValuePair<int, int>(offsetA, -1 * offsetB),
            new KeyValuePair<int, int>(-1 * offsetA, -1 * offsetB),
            new KeyValuePair<int, int>(offsetB, offsetA),
            new KeyValuePair<int, int>(-1 * offsetB, offsetA),
            new KeyValuePair<int, int>(offsetB, -1 * offsetA),
            new KeyValuePair<int, int>(-1 * offsetB, -1 * offsetA)
        };
    }

    #endregion

    #region Cardinal Direction Move

    public List<GridTile> GetTilesInCardinalDirections(GridTile start, MovementStyle style)
    {
        var toInstantiate = new List<GridTile>();

        start.CurrentMovementValue = style.actionCost;
        tilesToReset.Add(start);


        foreach (Directions direction in System.Enum.GetValues(typeof(Directions)))
        {
            toInstantiate.AddRange(GetValidTilesInCardinalDirection(direction, start, style, new List<GridTile>()));
        }

        return toInstantiate;
    }

    private List<GridTile> GetValidTilesInCardinalDirection(Directions direction, GridTile top, MovementStyle style, List<GridTile> accumulator)
    {
        var neighbor = GridController.instance.GetNeighborAt(direction, top.WorldLocation);
        if (neighbor != null && style.elligibleStartingStates.Contains(neighbor.State))
        {
            if (top.CurrentMovementValue - style.individualMoveCost >= 0 && top.CurrentMovementValue - style.individualMoveCost > neighbor.CurrentMovementValue)
            {
                neighbor.UpdateState(style.targetState);
                neighbor.CurrentMovementValue = top.CurrentMovementValue - neighbor.Cost;

                accumulator.Add(neighbor);
                return GetValidTilesInCardinalDirection(direction, neighbor, style, accumulator);
            }
        }

        return accumulator;
    }

    #region Charge Moves

    private List<GridTile> GetValidTilesInChargeDirection(GridTile start, MovementStyle style)
    {
        var toInstantiate = new List<GridTile>();

        //Can charge one additional tile past movement range
        start.CurrentMovementValue = style.actionCost + 1;
        tilesToReset.Add(start);


        foreach (Directions direction in System.Enum.GetValues(typeof(Directions)))
        {
            toInstantiate.AddRange(GetValidTilesInChargeDirection(direction, start, style, new List<GridTile>()));
        }

        return toInstantiate;
    }

    private List<GridTile> GetValidTilesInChargeDirection(Directions direction, GridTile top, MovementStyle style, List<GridTile> accumulator)
    {
        var neighbor = GridController.instance.GetNeighborAt(direction, top.WorldLocation);
        if (neighbor != null && style.elligibleStartingStates.Contains(neighbor.State))
        {
            var leftoverMovement = top.CurrentMovementValue - style.individualMoveCost;
            if (leftoverMovement >= 0 && leftoverMovement > neighbor.CurrentMovementValue)
            {
                if (leftoverMovement == 0 && neighbor.State != GridTile.MovementState.OCCUPIED)
                {
                    //At the max charge distance and unoccupied, don't include this tile
                    return accumulator;
                }

                if (neighbor.State == GridTile.MovementState.OCCUPIED)
                {
                    neighbor.CurrentMovementValue = 0;
                }
                else
                {
                    neighbor.CurrentMovementValue = top.CurrentMovementValue - neighbor.Cost;
                }

                neighbor.UpdateState(style.targetState);
                accumulator.Add(neighbor);
                return GetValidTilesInChargeDirection(direction, neighbor, style, accumulator);
            }
        }

        return accumulator;
    }

    #endregion

    #endregion

    #region Straight Direction Until Barrier Move

    public List<GridTile> GetMovesInDirectionUntilBarrier(GridTile start, MovementStyle style)
    {
        var instantiated = new List<GridTile>();

        var startX = start.WorldLocation.x;
        var startY = start.WorldLocation.y;

        var offsetsToTest = new List<KeyValuePair<int, int>>
        {
            new KeyValuePair<int, int>(0, 1),
            new KeyValuePair<int, int>(1, 1),
            new KeyValuePair<int, int>(1, 0),
            new KeyValuePair<int, int>(1, -1),
            new KeyValuePair<int, int>(0, -1),
            new KeyValuePair<int, int>(-1, -1),
            new KeyValuePair<int, int>(-1, 0),
            new KeyValuePair<int, int>(-1, 1)
        };

        foreach(KeyValuePair<int, int> offset in offsetsToTest)
        {
            var currentOffset = 0;
            GridTile elligble = null;
            do
            {
                var testX = offset.Key;
                var testY = offset.Value;
                if (offset.Key > 0)
                {
                    testX += currentOffset;
                } else if (offset.Key < 0)
                {
                    testX -= currentOffset;
                }
                if (offset.Value > 0)
                {
                    testY += currentOffset;
                } else if (offset.Value < 0)
                {
                    testY -= currentOffset;
                }

                var location = new Vector3(startX + testX, startY + testY);
                var tileAt = GridController.instance.GetTileAtPosition(location);
                if (tileAt != null && style.elligibleStartingStates.Contains(tileAt.State))
                {
                    elligble = tileAt;
                    currentOffset++;
                } else
                {
                    break;
                }
            } while (true);

            if (elligble != null)
            {
                instantiated.Add(elligble);
            }
        }


        return instantiated;
    }

    #endregion
}