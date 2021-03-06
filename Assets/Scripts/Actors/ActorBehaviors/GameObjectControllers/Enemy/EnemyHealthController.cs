﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthController : MonoBehaviour
{
    public int startingHealth = 10;
    private int currentHealth;

    bool damaged = false;
    bool isDead = false;
    public Text healthText;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = startingHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        healthText.text = currentHealth.ToString();
    }

    void Update()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            //spriteRenderer.color = tintColor;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
        }

        // Reset the damaged flag.
        damaged = false;
    }


    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        if (!isDead)
        {
            // Reduce the current health by the damage amount.
            currentHealth -= amount;
            // Set the health bar's value to the current health.
            healthText.text = currentHealth.ToString();
        }
        // Play the hurt sound effect.
        //playerAudio.Play ();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            healthText.text = currentHealth.ToString();
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        EventManager.TriggerEvent(EventNames.TERMINATE_MOVE);
        isDead = true;
        var onDeathAction = GetComponent<OnDeathTakeActionController>();
        if (onDeathAction != null)
        {
            ActionResolutionQueueController.instance.EnqueueEnemyAction(onDeathAction.OnDeath());
        } else
        {
            GridController.instance.MarkGridTileUnOccupied(transform.position);
            Destroy(gameObject);
        }
        // Turn off any remaining shooting effects.
        //playerShooting.DisableEffects ();

        //// Tell the animator that the player is dead.
        //anim.SetTrigger ("Die");

        //// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

        //// Turn off the movement and shooting scripts.
        //playerMovement.enabled = false;
        //playerShooting.enabled = false;
    }

    public void ReachGoal()
    {
        //isDead = true;
        //RosterController.instance.OnCharacterReachGoal(gameObject.name);
        //GridController.instance.MarkGridTileUnOccupied(transform.position);
        //Destroy(gameObject);
    }
}
