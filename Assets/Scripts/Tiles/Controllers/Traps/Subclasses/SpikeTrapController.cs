using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : TrapController
{
    public Sprite depressedState;
    public int damage;

    private SpriteRenderer spriteRenderer;
    private bool isDirty;
    private bool isDepressed;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.StartListening(EventNames.TRAPS_DEACTIVATED, OnTrapDeactivated);
    }

    private void Update()
    {
        if (isDirty && isDepressed)
        {
            spriteRenderer.sprite = depressedState;
            isDirty = false;
        }
    }

    private void OnTrapDeactivated(object arg0)
    {
        if (!isDepressed)
        {
            isDepressed = true;
            isDirty = true;
        }
    }

    public override void OnEnemyEnter(EnemyHealthController enemyHealth)
    {
        if (!isDepressed)
        {
            enemyHealth.TakeDamage(damage);
            EventManager.TriggerEvent(EventNames.TERMINATE_MOVE);
            GridController.instance.MarkTriggerForCleanup(transform.position);
        }
    }

    public override void OnPlayerEnter(PlayerHealthController playerHealth)
    {
        var playerCharacterController = playerHealth.gameObject.GetComponent<PlayerCharacterController>();
        if (!isDepressed && playerCharacterController.movementStyle.activatesTraps)
        {
            playerHealth.TakeDamage(damage);
            if (playerHealth.GetCurrentHealth() > 0)
            {
                EventManager.TriggerEvent(EventNames.TERMINATE_MOVE);
            } else {
                playerHealth.ReachGoal(transform.position);
            }
            GridController.instance.MarkTriggerForCleanup(transform.position);
        }
    }
}
