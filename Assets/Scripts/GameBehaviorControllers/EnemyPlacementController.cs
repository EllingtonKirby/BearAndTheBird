using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacementController : MonoBehaviour
{
    public static EnemyPlacementController instance;
    //public int enemyCount;
    //public List<GameObject> enemyPrefabs;
    //public List<float> enemySpawnChance;
    //public Vector3 startPosition;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

    }


    public void OnGridLayoutCompleted()
    {
        //var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //EnemyRosterController.instance.AddEnemiesToRoster(enemies);
        //var currentEnemyCount = EnemyRosterController.instance.enemyRoster.Count;
        //foreach (Vector3Int pos in baseGrid.cellBounds.allPositionsWithin)
        //{
        //    var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
        //    var tile = GridController.instance.GetTileAtPosition(baseGrid.GetCellCenterWorld(localPlace));
            
        //    if (tile != null && tile.State == GridTile.MovementState.DEFAULT)
        //    {
        //        //Found a space that is unoccupied and has no triggers
        //        if (GridController.instance.triggersMap.HasTile(localPlace) == false)
        //        {
        //            var roll = Random.Range(0, 101);
        //            foreach (float spawnChance in enemySpawnChance)
        //            {
        //                if (System.Math.Abs(roll % spawnChance) < Mathf.Epsilon)
        //                {
        //                    var enemy = Instantiate(enemyPrefabs[0], tile.WorldLocation, Quaternion.identity);
        //                    EnemyRosterController.instance.AddEnemyToRoster(enemy);
        //                    currentEnemyCount++;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    if (currentEnemyCount == enemyCount)
        //    {
        //        break;
        //    }
        //}

        //if (currentEnemyCount != enemyCount)
        //{
        //    OnGridLayoutCompleted();
        //}
        //else
        //{
        //    RosterController.instance.OnGridCompleted();
        //}
    }
}
