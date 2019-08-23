using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTurnExplodeAction : EnemyAction
{
    private readonly Explosions explosion;
    private readonly Transform parent;

    public OnTurnExplodeAction(Explosions explosion, Transform parent)
    {
        this.explosion = explosion;
        this.parent = parent;
    }

    public IEnumerator Perform()
    {
        var instantiated = Object.Instantiate(explosion.explosionPrefab, parent.position, Quaternion.identity);
        GridController.instance.MarkGridTileUnOccupied(parent.position);
        Object.Destroy(parent.gameObject);
        yield return new WaitWhile(() => instantiated.activeInHierarchy);
    }
}
