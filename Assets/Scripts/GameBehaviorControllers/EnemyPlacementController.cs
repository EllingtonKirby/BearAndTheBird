using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacementController : MonoBehaviour
{
    public static EnemyPlacementController instance;

    private Dictionary<EnemyController, List<GridTile>> positionToNearestCharacter;
    private Dictionary<EnemyController, PlayerCharacterController> nearestCharacter;

    /** What are we trying to do with this?
     *  We want to establish the position of each enemy relative to the closest
     *  player character
     *  We want to establish this distance in terms of Grids, not raw magnitude
     *
     *  After caching this distance, we can then use it in two places
     *
     *  First to determine what an enemies action should be, depdening on it's
     *  proximity to an enemy
     *
     *  Seconds, is in the sorting of the enemies at the beginning of their turn
     */


    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

    }

    private void Start()
    {
        positionToNearestCharacter = new Dictionary<EnemyController, List<GridTile>>();
        nearestCharacter = new Dictionary<EnemyController, PlayerCharacterController>();
    }


    public void EstablishEnemyPositions()
    {
        positionToNearestCharacter.Clear();
        nearestCharacter.Clear();
        var currentRoster = EnemyRosterController.instance.GetCurrentRoster();
        var currentCharacters = GameObject.FindGameObjectsWithTag("Player");

        foreach(EnemyController enemy in currentRoster)
        {
            var pos = new List<GridTile>();
            var start = GridController.instance.GetTileAtPosition(enemy.transform.position);
            foreach(GameObject player in currentCharacters)
            {
                if (!player.activeInHierarchy)
                {
                    continue;
                }
                var end = GridController.instance.GetTileAtPosition(player.transform.position);
                var toChar = AStarHelper.GetPath(start, end, true);
                if (pos.Count == 0 || pos.Count > toChar.Count)
                {
                    pos = toChar;
                    nearestCharacter[enemy] = player.GetComponent<PlayerCharacterController>();
                }
            }
            positionToNearestCharacter[enemy] = pos;
        }
    }

    public List<GridTile> GetEnemyPathToNearestCharacter(EnemyController enemy)
    {
        return positionToNearestCharacter[enemy];
    }

    public PlayerCharacterController GetNearestCharacter(EnemyController enemy)
    {
        return nearestCharacter[enemy];
    }
}
