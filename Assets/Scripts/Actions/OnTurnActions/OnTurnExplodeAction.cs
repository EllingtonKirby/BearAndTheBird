using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTurnExplodeAction : EnemyAction
{
    private readonly Explosions explosion;
    private readonly Transform parent;
	private readonly bool isTurnAction;

    public OnTurnExplodeAction(Explosions explosion, Transform parent, bool isTurnAction)
    {
        this.explosion = explosion;
        this.parent = parent;
		this.isTurnAction = isTurnAction;
    }

    public IEnumerator Perform()
    {
        var instantiated = Object.Instantiate(explosion.explosionPrefab, parent.position, Quaternion.identity);
        GridController.instance.MarkGridTileUnOccupied(parent.position);
        Object.Destroy(parent.gameObject);
        //This code ensures that the end move trigger only fires on explosions that occur on the piece's turn
        var triggerEndMove = isTurnAction;
        if (!triggerEndMove)
        {
            yield return new WaitWhile(() => instantiated != null && instantiated.activeInHierarchy);
        }
        else
        {
            while (triggerEndMove)
            {
                yield return new WaitWhile(() => instantiated != null && instantiated.activeInHierarchy);
                EventManager.TriggerEvent(EventNames.END_MOVE);
                triggerEndMove = false;
            }
            yield return null;
        }
    }
}
