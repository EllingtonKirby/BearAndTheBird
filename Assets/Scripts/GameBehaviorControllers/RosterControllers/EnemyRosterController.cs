using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRosterController : MonoBehaviour
{
    public static EnemyRosterController instance;

    public List<EnemyController> enemyRoster;

    private int currentTurnPointer;
    private EnemyTurnComparer enemyComparer = new EnemyTurnComparer();


    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    public void AddEnemyToRoster(GameObject enemy)
    {
        enemyRoster.Add(enemy.GetComponent<EnemyController>());
    }

    public void InitTurnIterator()
    {
        currentTurnPointer = 0;
        enemyComparer.Init();
        enemyRoster.Sort(enemyComparer);
    }

    public void TakeEnemyTurn()
    {
        EventManager.StartListening(
            EventNames.END_MOVE,
            TakeNextAction
        );
        TakeNextAction(null);
    }

    public void TakeNextAction(object arg0)
    {
        StartCoroutine(ProcessActions());
    }

    private IEnumerator ProcessActions()
    {
        yield return StartCoroutine(ActionResolutionQueueController.instance.YieldToActionResolution());

        if (currentTurnPointer == enemyRoster.Count)
        {
            EventManager.StopListening(
                EventNames.END_MOVE,
                TakeNextAction
            );
            EventManager.TriggerEvent(EventNames.ENEMY_TURN_COMPLETED);
            yield return null;
        }
        else
        {
            var enemy = enemyRoster[currentTurnPointer];
            if (enemy != null)
            {
                yield return StartCoroutine(enemy.GetAction().Perform());
                currentTurnPointer++;
            }
            else
            {
                enemyRoster.RemoveAt(currentTurnPointer);
                EventManager.TriggerEvent(EventNames.END_MOVE);
                yield return null;
            }
        }
    }
}
