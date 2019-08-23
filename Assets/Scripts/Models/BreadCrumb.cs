using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BreadCrumb
{
    public Vector2 position;
    public int movementCost;

    public BreadCrumb(Vector2 position, int movementCost)
    {
        this.position = position;
        this.movementCost = movementCost;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is BreadCrumb))
        {
            return false;
        }

        var crumb = (BreadCrumb)obj;
        return position.Equals(crumb.position) &&
               movementCost == crumb.movementCost;
    }

    public override int GetHashCode()
    {
        var hashCode = 1263786711;
        hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(position);
        hashCode = hashCode * -1521134295 + movementCost.GetHashCode();
        return hashCode;
    }
}
