using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public class Event : UnityEvent<System.Object> { }

    private Dictionary<string, Event> eventDictionary;
    public static EventManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        Init();
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Event>();
        }
    }

    public static void StartListening(string eventName, UnityAction<System.Object> listener)
    {
        Event thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new Event();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<System.Object> listener)
    {
        if (instance == null) return;
        Event thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, System.Object arg = null)
    {
        Event thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(arg);
        }
    }

    public static void TriggerMouseEvent(string eventName, MouseEventActionProvider actionProvider)
    {
        TriggerEvent(eventName, actionProvider);
    }
}
