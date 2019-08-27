using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTileController : MonoBehaviour
{
    public GameObject characterToSpawn;

    void Start()
    {
        var created = Instantiate(characterToSpawn, transform.position, Quaternion.identity);
        if (characterToSpawn.tag == "Enemy")
        {
            EnemyRosterController.instance.AddEnemyToRoster(created);
        }
    }
}
