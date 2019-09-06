using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionPointSliderController : MonoBehaviour
{

    Slider mainSlider;
    Text label;

    // Start is called before the first frame update
    void Start()
    {
        mainSlider = GetComponentInChildren<Slider>();
        label = GetComponentInChildren<Text>();
        EventManager.StartListening(EventNames.UI_DEBIT_TOTAL_ACTION_POINTS, OnActionPointsChanged);
        EventManager.StartListening(EventNames.UI_LEVEL_LOAD, Initalize);
    }


    public void OnActionPointsChanged(object argument)
    {
        var player = argument as Player;
        var amount = player.ActionPointReserve;
        mainSlider.value = amount;
        label.text = GetActionPointsText(amount);
    }

    private void Initalize(object argument)
    {
        var player = argument as Player;
        mainSlider.maxValue = player.ActionPointReserve;
        OnActionPointsChanged(player);
    }

    private string GetActionPointsText(int amount)
    {
        return amount + " Total Action Points Remaining";
    }
}
