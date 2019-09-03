using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerAdjacentPrimeForExplodeController : OnPlayerAdjacentActionController
{
    public Sprite spriteToSet;

    public override OnAdjacentPlayerAction GetOnPlayerAdjacentAction(Vector3 position)
    {
        var spriteRenderer = GetComponentInParent<SpriteRenderer>();
        var onExplodeController = GetComponentInParent<OnExplodeController>();
        onExplodeController.PrimeForExplosion();
        return new ChangeSpriteOnAdjacentAction(spriteRenderer, spriteToSet);
    }
}
