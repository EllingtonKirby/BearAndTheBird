using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapController : TrapController
{
    public int damage;

    public override void OnEnemyEnter(EnemyHealthController enemyHealth)
    {
        Debug.Log("### Enemy entered spike trap");
        enemyHealth.TakeDamage(damage);
        EventManager.TriggerEvent(EventNames.TERMINATE_MOVE);
        Destroy(gameObject);
        GridController.instance.DestroyTriggerAtPosition(transform.position);
    }

    public override void OnPlayerEnter(PlayerHealthController playerHealth)
    {
        var playerCharacterController = playerHealth.gameObject.GetComponent<PlayerCharacterController>();
        if (playerCharacterController.movementStyle.activatesTraps)
        {
            playerHealth.TakeDamage(damage);
            EventManager.TriggerEvent(EventNames.TERMINATE_MOVE);
            Destroy(gameObject);
            GridController.instance.DestroyTriggerAtPosition(transform.position);
        }
    }
}
