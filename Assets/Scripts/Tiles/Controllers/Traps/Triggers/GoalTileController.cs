using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTileController : TrapController
{

    /**
     * We want this to work on a per character basis
     * 
     * Probably need an enum to distinguish characters, that we set on the player character controller.
     * Set the corresponding enum on this controller
     * That enum determines the sprite we load on this, can then determine different properties of the tile for the secondary behaviors
     * When a player enters, check the enum, only trigger goals for characters that match the enum
     */

    public CharacterTypes acceptedType;
    public List<Sprite> spriteVariants;

    private SpriteRenderer spriteRenderer;

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteVariants[(int) acceptedType];
    }


    public override void OnEnemyEnter(EnemyHealthController enemyHealth)
    {
        enemyHealth.ReachGoal();
    }

    public override void OnPlayerEnter(PlayerHealthController playerHealth)
    {
        playerHealth.ReachGoal(transform.position);
    }
}
