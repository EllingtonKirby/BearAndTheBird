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


    PlayerHealthController enteredCharacter;
    private string tileEventName;
    private bool stopListening;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        tileEventName = string.Format(EventNames.MOVED_TO_POSITION, transform.position.x, transform.position.y);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteVariants[(int) acceptedType];
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
        var playerController = playerHealth.GetComponentInParent<PlayerCharacterController>();
        if (playerController.type == acceptedType)
        {
            enteredCharacter = playerHealth;
            EventManager.StartListening(tileEventName, OnCharacterMovementEvent);
        }
    }

    private void OnCharacterMovementEvent(object arg0) {
        enteredCharacter.ReachGoal(transform.position);
        stopListening = true;
    }
}
