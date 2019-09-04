using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapController : MonoBehaviour
{

    GameObject enteredCharacter;
    private string tileEventName;
    private bool stopListening;

    public virtual void Start()
    {
        tileEventName = string.Format(EventNames.MOVED_TO_POSITION, transform.position.x, transform.position.y);
        EventManager.StartListening(tileEventName, OnCharacterMovementEvent);
        Debug.Log("Listening for " + tileEventName);
    }

    private void Update()
    {
        //if (stopListening)
        //{
        //    EventManager.StopListening(tileEventName, OnCharacterMovementEvent);
        //    enteredCharacter = null;
        //    stopListening = false;
        //}
    }

    private void OnCharacterMovementEvent(object arg0)
    {
        if (enteredCharacter.gameObject.tag == "Player")
        {
            var healthController = enteredCharacter.gameObject.GetComponent<PlayerHealthController>();
            OnPlayerEnter(healthController);
        }
        if (enteredCharacter.gameObject.tag == "Enemy")
        {
            var enemyHealthController = enteredCharacter.gameObject.GetComponent<EnemyHealthController>();
            OnEnemyEnter(enemyHealthController);
        }
        //stopListening = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        enteredCharacter = other.gameObject;
    }

    public abstract void OnPlayerEnter(PlayerHealthController playerHealth);

    public abstract void OnEnemyEnter(EnemyHealthController enemyHealth);
}
