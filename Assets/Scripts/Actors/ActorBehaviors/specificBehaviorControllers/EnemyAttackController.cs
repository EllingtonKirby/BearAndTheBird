using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<PlayerCharacterController>();
            var playerHealth = collision.gameObject.GetComponent<PlayerHealthController>();
            playerHealth.TakeDamage(damage);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            var enemy = collision.gameObject.GetComponent<EnemyController>();
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealthController>();
            enemyHealth.TakeDamage(damage);
        }
    }
}
