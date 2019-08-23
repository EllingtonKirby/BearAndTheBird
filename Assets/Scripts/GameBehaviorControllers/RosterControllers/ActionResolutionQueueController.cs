using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionResolutionQueueController : MonoBehaviour
{
    public static ActionResolutionQueueController instance;

    private Queue<Action> playerActionQueue;
    private Queue<EnemyAction> enemyActionQueue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerActionQueue = new Queue<Action>();
        enemyActionQueue = new Queue<EnemyAction>();
    }

    public void EnqueuePlayerAction(Action action)
    {
        lock (playerActionQueue)
        {
            playerActionQueue.Enqueue(action);
        }
    }

    public void EnqueueEnemyAction(EnemyAction action)
    {
        lock(enemyActionQueue)
        {
            enemyActionQueue.Enqueue(action);
        }
    }

    public IEnumerator YieldToActionResolution()
    {
        while (playerActionQueue.Count > 0)
        {
            var top = playerActionQueue.Dequeue();
            yield return StartCoroutine(top.Perform());
        }
        yield return new WaitForEndOfFrame();
        while (enemyActionQueue.Count > 0)
        {
            Debug.Log("Consuming Enemy Side Effect Action");
            var top = enemyActionQueue.Dequeue();
            yield return StartCoroutine(top.Perform());
        }

        yield return null;
    }
}
