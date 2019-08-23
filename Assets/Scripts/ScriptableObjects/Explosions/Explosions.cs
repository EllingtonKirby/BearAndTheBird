using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "Explosion")]
public class Explosions: ScriptableObject
{
    //Number of distance to extend to caluclate damage
    public int radius;
    public int damage;
    public GameObject explosionPrefab;
}
