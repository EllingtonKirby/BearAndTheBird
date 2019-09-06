using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStoreController : MonoBehaviour, DoesOnLevelStart
{
    public static LevelStoreController instance;

    Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void Instantiate()
    {
        player = new Player();
    }
}
