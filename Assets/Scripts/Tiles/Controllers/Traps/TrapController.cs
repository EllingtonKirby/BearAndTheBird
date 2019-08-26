using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapController : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            var healthController = other.gameObject.GetComponent<PlayerHealthController>();
            OnPlayerEnter(healthController);
        }
        if (other.gameObject.tag == "Enemy")
        {
            var enemyHealthController = other.gameObject.GetComponent<EnemyHealthController>();
            OnEnemyEnter(enemyHealthController);
        }
    }

    public abstract void OnPlayerEnter(PlayerHealthController playerHealth);

    public abstract void OnEnemyEnter(EnemyHealthController enemyHealth);
}
