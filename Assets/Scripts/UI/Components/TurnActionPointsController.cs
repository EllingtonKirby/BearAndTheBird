﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnActionPointsController : MonoBehaviour
{
    Text label;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventNames.UI_ACTION_POINT_CONSUMED, DebitTurnActionPoint);
        EventManager.StartListening(EventNames.UI_USER_END_TURN, ShowNoMoreTurnActionPoints);
        EventManager.StartListening(EventNames.UI_ENEMY_END_TURN, ResetTurnActionPoints);
    }

    private void DebitTurnActionPoint(object argument)
    {
        label.text = GetLabel(TurnController.instance.GetActionPointsRemaining());
    }

    private void ShowNoMoreTurnActionPoints(object argument)
    {
        label.text = GetLabel(0);
    }

    private void ResetTurnActionPoints(object argument)
    {
        var player = argument as Player;
        label.text = GetLabel(player.ActionPointsPerTurn);
    }

    private string GetLabel(int points)
    {
        if (points == 0)
        {
            return "Waiting for enemy turn...";
        } else
        {
            return points + " Action Points remaining this turn";
        }
    }
}
