using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //var eventName = string.Format(EventNames.MOVED_TO_POSITION, transform.position.x, transform.position.y);
        //EventManager.StartListening(eventName, OnMovedToThisPosition);
        //Debug.Log("TRAP LISTENING TO " + eventName);
    }

    //private void Update()
    //{
    //    Collider2D hit = Physics2D.OverlapBox(this.transform.position, new Vector2(.9f, .8f), 0f);
    //    if (hit != null)
    //    {
    //        if (hit.transform.gameObject.tag == "Player")
    //        {
    //            var healthController = hit.gameObject.GetComponent<PlayerHealthController>();
    //            OnPlayerEnter(healthController);
    //        }
    //        if (hit.transform.gameObject.tag == "Enemy")
    //        {
    //            var enemyHealthController = hit.gameObject.GetComponent<EnemyHealthController>();
    //            OnEnemyEnter(enemyHealthController);
    //        }
            
    //    }
    //}

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


    public void OnMovedToThisPosition(object arg0)
    {
    }

    public abstract void OnPlayerEnter(PlayerHealthController playerHealth);

    public abstract void OnEnemyEnter(EnemyHealthController enemyHealth);
}
