using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : TrapController
{

    public Sprite depressedState;

    private SpriteRenderer spriteRenderer;
    private bool isDepressed;
    private bool isDirty;

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    private void Update()
    {
        if (isDirty && isDepressed)
        {
            spriteRenderer.sprite = depressedState;
            isDirty = false;
        }
    }


    public override void OnEnemyEnter(EnemyHealthController enemyHealth)
    {
    }

    public override void OnPlayerEnter(PlayerHealthController playerHealth)
    {
        var playerCharacterController = playerHealth.gameObject.GetComponent<PlayerCharacterController>();
        if (playerCharacterController.movementStyle.activatesTraps)
        {
            if (!isDepressed)
            {
                isDepressed = true;
                isDirty = true;
                EventManager.TriggerEvent(EventNames.TRAPS_DEACTIVATED);
            }
        }
    }
}
