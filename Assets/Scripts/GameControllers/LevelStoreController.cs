using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStoreController : MonoBehaviour
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

    void Start()
    {
        player = new Player();
    }

    public Player GetPlayer()
    {
        return player;
    }
}
