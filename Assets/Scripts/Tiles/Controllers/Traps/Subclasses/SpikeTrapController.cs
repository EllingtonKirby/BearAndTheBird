using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : TrapController
{
    public int damage;

    public override void OnEnemyEnter(EnemyHealthController enemyHealth)
    {
        enemyHealth.TakeDamage(damage);
        EventManager.TriggerEvent(EventNames.TERMINATE_MOVE);
        GridController.instance.MarkTriggerForCleanup(transform.position);
    }

    public override void OnPlayerEnter(PlayerHealthController playerHealth)
    {
        var playerCharacterController = playerHealth.gameObject.GetComponent<PlayerCharacterController>();
        if (playerCharacterController.movementStyle.activatesTraps)
        {
            playerHealth.TakeDamage(damage);
            EventManager.TriggerEvent(EventNames.TERMINATE_MOVE);
            GridController.instance.MarkTriggerForCleanup(transform.position);
        }
    }
}
