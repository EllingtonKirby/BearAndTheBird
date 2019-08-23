using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNoMovementAvalableAction : Action
{
    private Vector3 position;
    private int instanceId;

    public ShowNoMovementAvalableAction(Vector3 position, int instanceId)
    {
        this.position = position;
        this.instanceId = instanceId;
    }

    public IEnumerator Perform()
    {
        var eventName = string.Format(EventNames.CHARACTER_TEXT_DISPLAY, instanceId);
        EventManager.TriggerEvent(eventName, "No Movement Available");
        yield return null;
    }
}