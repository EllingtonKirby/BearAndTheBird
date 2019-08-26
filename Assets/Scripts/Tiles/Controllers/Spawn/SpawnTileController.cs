using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTileController : MonoBehaviour
{
    public GameObject characterToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(characterToSpawn, transform.position, Quaternion.identity);
    }
}
