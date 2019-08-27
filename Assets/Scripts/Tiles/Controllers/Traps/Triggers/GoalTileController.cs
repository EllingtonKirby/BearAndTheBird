using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTileController : TrapController
{

    PlayerHealthController enteredCharacter;
    private string tileEventName;
    private bool stopListening;

    private void Start()
    {
        tileEventName = string.Format(EventNames.MOVED_TO_POSITION, transform.position.x, transform.position.y);
    }

    private void Update()
    {
        if (stopListening)
        {
            EventManager.StopListening(tileEventName, OnCharacterMovementEvent);
            enteredCharacter = null;
            stopListening = false;
        }   
    }

    public override void OnEnemyEnter(EnemyHealthController enemyHealth)
    {
        enemyHealth.ReachGoal();
    }

    public override void OnPlayerEnter(PlayerHealthController playerHealth)
    {
        enteredCharacter = playerHealth;
        EventManager.StartListening(tileEventName, OnCharacterMovementEvent);
    }

    private void OnCharacterMovementEvent(object arg0) {
        enteredCharacter.ReachGoal(transform.position);
        stopListening = true;
    }
}
