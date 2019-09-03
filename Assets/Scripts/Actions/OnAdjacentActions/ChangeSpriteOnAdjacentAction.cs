using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteOnAdjacentAction : OnAdjacentPlayerAction
{
    private readonly SpriteRenderer spriteRenderer;
    private readonly Sprite spriteToSet;

    public ChangeSpriteOnAdjacentAction(SpriteRenderer spriteRenderer, Sprite spriteToSet)
    {
        this.spriteRenderer = spriteRenderer;
        this.spriteToSet = spriteToSet;
    }

    public IEnumerator Perform()
    {
        spriteRenderer.sprite = spriteToSet;
        yield return null;
    }
}
