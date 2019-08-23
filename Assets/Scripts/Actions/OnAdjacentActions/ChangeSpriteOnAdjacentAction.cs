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

    public void Perform()
    {
        spriteRenderer.sprite = spriteToSet;
    }
}
