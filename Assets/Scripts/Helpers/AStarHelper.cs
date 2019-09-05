using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directions = GridController.NeighboringGrids;
public static class AStarHelper
{
    public static List<GridTile> GetPath(GridTile start, GridTile goal, bool isRawCount = false)
    {
        if (start == null)
        {
            return new List<GridTile>();
        }
        //Evaluated nodes
        var closedSet = new HashSet<GridTile>();

        //Currently discovered nodes that have not been evaluated yet
        var openSet = new HashSet<GridTile>();
        openSet.Add(start);

        //Priority queue that sorts by lowest fScore
        var openSetbyFScore = new SimplePriorityQueue();

        // For each node, which node it can most efficiently be reached from.
        // If a node can be reached from many nodes, cameFrom will eventually contain the
        // most efficient previous step.
        var cameFrom = new Dictionary<GridTile, GridTile>();

        // For each node, the cost of getting from the start node to that node with default value of Infinity
        var gScore = new Dictionary<GridTile, int>();
        gScore[start] = 0;

        // For each node, the total cost of getting from the start node to the goal
        // by passing by that node. That value is partly known, partly heuristic.
        // with default value of Infinity
        var fScore = new Dictionary<GridTile, float>();
        fScore[start] = GetHeuristicEstimate(start, goal) - 1f;
        openSetbyFScore.Add(start, fScore[start]);
        while (openSet.Count > 0)
        {
            var current = openSetbyFScore.GetTop();
            
            if (current == goal)
            {
                return ReconstructPath(current, cameFrom, isRawCount);
            }

            openSet.Remove(current);
            openSetbyFScore.Remove(current);
            closedSet.Add(current);
            foreach (Directions direction in System.Enum.GetValues(typeof(Directions)))
            {
                var neighbor = GridController.instance.GetNeighborAt(direction, current.WorldLocation);

                if (neighbor == null || closedSet.Contains(neighbor) || neighbor.State == GridTile.MovementState.COLLIDER)
                {
                    if (!goal.Equals(neighbor))
                    {
                        continue;
                    } else
                    {
                        //We have gotten to the goal via the closest unoccupied neighbor
                        return ReconstructPath(current, cameFrom, isRawCount);
                    }
                }

                var tentativeGScore = gScore[current] + GetDistanceBetween(current, neighbor);
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor))
                {
                    continue;
                }
                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + GetHeuristicEstimate(neighbor, goal);
                openSetbyFScore.Add(neighbor, fScore[neighbor]);
            }
        }
        return new List<GridTile>();
    }

    private static List<GridTile> ReconstructPath(GridTile current, Dictionary<GridTile, GridTile> cameFrom, bool isRawCount)
    {
        var totalList = new List<GridTile>();
        totalList.Add(current);
        Debug.DrawLine(current.WorldLocation, cameFrom[current].WorldLocation, Color.white, 10);
        var next = cameFrom[current];
        while (next != null && cameFrom.ContainsKey(next))
        {
            totalList.Add(next);
            next = cameFrom[next];
        }
        totalList.Reverse();
        if (!isRawCount)
        {
            var progressableTiles = new List<GridTile>();
            foreach (GridTile tile in totalList)
            {
                if (tile.State == GridTile.MovementState.OCCUPIED)
                {
                    break;
                }
                else
                {
                    progressableTiles.Add(tile);
                }
            }
            return progressableTiles;
        } else
        {
            return totalList;
        }

    }

    private static float GetHeuristicEstimate(GridTile current, GridTile goal)
    {
        var magnitude = Vector2.Distance(current.WorldLocation, goal.WorldLocation);
        //Want to make occupied tiles less appealing, but still possible to navigate through
        if (current.State == GridTile.MovementState.OCCUPIED)
        {
            magnitude += .3f;
        }

        return magnitude;
    }

    private static int GetDistanceBetween(GridTile current, GridTile neighbor)
    {
        //Could do interesting things with action points here
        return 1;
    }

    public static int GetValueOrDefault(this Dictionary<GridTile, int> dictionary, GridTile key)
    {
        int value;
        return dictionary.TryGetValue(key, out value) ? value : int.MaxValue;
    }
}
