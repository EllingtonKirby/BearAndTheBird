using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{

    public int startingHealth = 10;
    private int currentHealth;
    public string healthTextName;
    public string healthLabelTextName;

    bool damaged = false;
    bool isDead = false;
    Text healthText;
    Text healthLabelText;
    SpriteRenderer spriteRenderer;
    Vector3? terminateAtPosition;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = startingHealth;
        healthText = GameObject.Find(healthTextName).GetComponent<Text>();
        healthLabelText = GameObject.Find(healthLabelTextName).GetComponent<Text>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        healthText.text = currentHealth.ToString();
    }

    void Update ()
    {
        if(terminateAtPosition != null)
        {
            if (transform.position.Equals(terminateAtPosition))
            {
                if (isDead)
                {
                    RosterController.instance.OnCharacterDeath(gameObject.name);
                } else
                {
                    RosterController.instance.OnCharacterReachGoal(gameObject.name);
                }
                EventManager.TriggerEvent(EventNames.TERMINATE_MOVE);
                GridController.instance.MarkGridTileUnOccupied(transform.position);
                terminateAtPosition = null;
                Destroy(gameObject);
            }
        } else if (isDead)
        {
            RosterController.instance.OnCharacterDeath(gameObject.name);
            GridController.instance.MarkGridTileUnOccupied(transform.position);
            terminateAtPosition = null;
            Destroy(gameObject);
        }

        damaged = false;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage (int amount)
    {
        Debug.Log("Taking damage");
        damaged = true;

        if (!isDead)
        {
            currentHealth -= amount;
            healthText.text = currentHealth.ToString();
        }
        // Play the hurt sound effect.
        //playerAudio.Play ();

        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            healthText.text = currentHealth.ToString();
            
            Death();
        }
    }


    void Death ()
    {
		// Set the death flag so this function won't be called again.
        
		isDead = true;
        EnemyPlacementController.instance.EstablishEnemyPositions();
        
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

    public void ReachGoal(Vector3 goalPosition)
    {
        terminateAtPosition = goalPosition; 
    }
}
