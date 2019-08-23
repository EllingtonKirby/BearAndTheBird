using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePriorityQueue
{
    //private List<KeyValuePair<GridTile, int>> data;
    //private HashSet<GridTile> valuesInSet;

    //public GridTile GetTop()
    //{
    //    var top = data[data.Count - 1].Key;
    //    data.RemoveAt(data.Count - 1);
    //    return top;
    //}

    //public void Add(GridTile tile, int fScore)
    //{
    //    var toAdd = new KeyValuePair<GridTile, int>(tile, fScore);
    //    if (data.Count == 0)
    //    {
    //        data.Add(toAdd);
    //        return;
    //    }
    //    for (int i = data.Count - 1; i > 0; i--)
    //    {
    //        if (fScore < data[i].Value || i == 0)
    //        {
    //            data.Insert(i + 1, toAdd);
    //        }
    //        else 
    //        {
    //            continue;
    //        }
    //    }
    //}

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
