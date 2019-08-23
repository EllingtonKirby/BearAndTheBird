using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterExplodeDestroy : MonoBehaviour
{
    private bool shouldDestroy = false;


    private void Update()
    {
        if (shouldDestroy)
        {
            Destroy(transform.parent.gameObject);
            EventManager.TriggerEvent(EventNames.END_MOVE);
        }
    }

    public void AfterExplode()
    {
        Debug.Log("After Explode called");
        shouldDestroy = true;
    }
}
