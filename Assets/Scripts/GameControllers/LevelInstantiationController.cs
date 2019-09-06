using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInstantiationController : MonoBehaviour
{
    void Start()
    {
        LevelStoreController.instance.Instantiate();
        GridController.instance.Instantiate();
        RosterController.instance.Instantiate();
        TurnController.instance.Instantiate();
        UiInteractionController.instance.Instantiate();
        EventManager.TriggerEvent(EventNames.UI_LEVEL_LOAD, LevelStoreController.instance.GetPlayer());
    }
}
