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
        mainSlider = GetComponent<Slider>();
        label = GetComponent<Text>();
        EventManager.StartListening(EventNames.UI_ACTION_POINT_CONSUMED, OnActionPointsChanged);
        Initalize();
    }


    public void OnActionPointsChanged(object argument)
    {
        var player = argument as Player;
        var amount = player.ActionPointReserve;
        mainSlider.value = amount;
        label.text = GetActionPointsText(amount);
    }

    private void Initalize()
    {
        var player = LevelStoreController.instance.GetPlayer();
        OnActionPointsChanged(player);
    }

    private string GetActionPointsText(int amount)
    {
        return amount + " Remaining";
    }
}
