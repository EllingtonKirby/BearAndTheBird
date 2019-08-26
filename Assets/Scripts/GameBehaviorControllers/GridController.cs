﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    public static GridController instance;

    public Tilemap tileMap;
    public Tilemap collidersMap;
    public Tilemap triggersMap;

    private Dictionary<Vector3, GridTile> triggers;
    private Dictionary<Vector3, GridTile> tiles;
    private Dictionary<Vector3, GridTile> colliders;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

    }

    void Start()
    {
        GetWorldTiles();
    }

    private void GetWorldTiles()
    {
        tiles = new Dictionary<Vector3, GridTile>();
        triggers = new Dictionary<Vector3, GridTile>();
        colliders = new Dictionary<Vector3, GridTile>();

        foreach (Vector3Int pos in tileMap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (!tileMap.HasTile(localPlace)) continue;
            var tile = new GridTile
            {
                LocalPlace = localPlace,
                WorldLocation = tileMap.GetCellCenterWorld(localPlace),
                TileBase = tileMap.GetTile(localPlace),
                TilemapMember = tileMap,
                Name = localPlace.x + "," + localPlace.y,
                Cost = 1,
                State = GridTile.MovementState.DEFAULT,
                CurrentMovementValue = int.MinValue
            };
            tiles.Add(tile.WorldLocation, tile);
        }

        var bounds = new BoundsInt(triggersMap.origin, triggersMap.size);
        foreach (var pos in bounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (!triggersMap.HasTile(localPlace)) continue;
            Debug.Log("Trigger tile found, creating data for it");
            var tile = new GridTile
            {
                LocalPlace = localPlace,
                WorldLocation = triggersMap.GetCellCenterWorld(localPlace),
                TileBase = triggersMap.GetTile(localPlace),
                TilemapMember = triggersMap,
                Name = localPlace.x + "," + localPlace.y,
                Cost = 1,
                State = GridTile.MovementState.DEFAULT,
                CurrentMovementValue = int.MinValue
            };
            triggers.Add(tile.WorldLocation, tile);
        }

        foreach (Vector3Int pos in collidersMap.cellBounds.allPositionsWithin)
        {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (!collidersMap.HasTile(localPlace)) continue;
            var tile = new GridTile
            {
                LocalPlace = localPlace,
                WorldLocation = collidersMap.GetCellCenterWorld(localPlace),
                TileBase = collidersMap.GetTile(localPlace),
                TilemapMember = collidersMap,
                Name = localPlace.x + "," + localPlace.y,
                Cost = 1,
                State = GridTile.MovementState.OCCUPIED,
                CurrentMovementValue = int.MinValue              
            };
            colliders.Add(tile.WorldLocation, tile);
        }

        EnemyPlacementController.instance.OnGridLayoutCompleted();
    }

    public bool DestroyTriggerAtPosition(Vector3 origin)
    {
        if (triggers.ContainsKey(origin))
        {
            var tile = triggers[origin];
            triggersMap.SetTile(tile.LocalPlace, null);
            triggers.Remove(origin);
            return true;
        } else
        {
            return false;
        }
    }

    public GridTile GetTileAtPosition(Vector3 origin)
    {
        var worldPosition = origin;
        if (triggers.ContainsKey(worldPosition))
        {
            return triggers[worldPosition];
        }
        else if (colliders.ContainsKey(worldPosition))
        {
            return colliders[worldPosition];
        }
        else if (tiles.ContainsKey(worldPosition))
        {
            return tiles[worldPosition];
        }
        else return null;
    }

    public GridTile GetNeighborAt(NeighboringGrids direction, Vector3 initialLocation)
    {
        Vector3 newDirection;
        switch (direction)
        {
            case NeighboringGrids.NORTH:
                newDirection = new Vector3(initialLocation.x, initialLocation.y + 1, initialLocation.z);
                break;
            case NeighboringGrids.EAST:
                newDirection = new Vector3(initialLocation.x + 1, initialLocation.y, initialLocation.z);
                break;
            case NeighboringGrids.SOUTH:
                newDirection = new Vector3(initialLocation.x, initialLocation.y - 1, initialLocation.z);
                break;
            case NeighboringGrids.WEST:
                newDirection = new Vector3(initialLocation.x - 1, initialLocation.y, initialLocation.z);
                break;
            default:
                newDirection = initialLocation;
                break;
        }
        return GetTileAtPosition(newDirection);
    }

    public void MarkGridTileOccupied(Vector3 position)
    {
        var tile = GetTileAtPosition(position);
        if (tile != null)
        {
            tile.State = GridTile.MovementState.OCCUPIED;
        }
    }

    public void MarkGridTileUnOccupied(Vector3 position)
    {
        var tile = GetTileAtPosition(position);
        if (tile != null)
        {
            tile.State = GridTile.MovementState.DEFAULT;
        }
    }

    public enum NeighboringGrids
    {
        NORTH = 0, EAST, SOUTH, WEST
    }
}
