using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePriorityQueue
{
    private Dictionary<GridTile, float> values = new Dictionary<GridTile, float>();

    public void Add(GridTile tile, float fScore)
    {
        if (values.ContainsKey(tile))
        {
            values[tile] = fScore;
        }
        else
        {
            values.Add(tile, fScore);
        }
    }

    public void Remove(GridTile tile)
    {
        values.Remove(tile);
    }

    public GridTile GetTop()
    {
        var min = float.MaxValue;
        GridTile current = null;
        foreach (GridTile key in values.Keys)
        {
            if (values[key] < min)
            {
                min = values[key];
                current = key;
            }
        }
        return current;
    }
}
