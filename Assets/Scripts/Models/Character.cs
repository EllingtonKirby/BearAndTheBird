using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
    public string Name { get; private set; }

    public int Health { get; private set; }


    CharacterTypes type;
    MovementStyle movementStyle;

    public void ChangeHealth(int amount)
    {
        Health = amount;
        EventManager.TriggerEvent(EventNames.UI_CHARACTER_HEALTH_CHANGE, this);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        EventManager.TriggerEvent(EventNames.UI_CHARACTER_HEALTH_CHANGE, this);
    }
}
