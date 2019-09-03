using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDebugSquareAction : OnAdjacentPlayerAction
{
    private readonly Vector3 position;

    public ShowDebugSquareAction(Vector3 position)
    {
        this.position = position;
    }


    public IEnumerator Perform()
    {
        var position1 = new Vector3(position.x -.25f, position.y);
        var position2 = new Vector3(position.x + .25f, position.y);
        Debug.DrawLine(position1, position2, Color.white, 2);
        Debug.Log("OnPlayerAdjacent called, at position " + position);
        yield return null;
    }
}
