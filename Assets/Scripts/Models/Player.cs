using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ScriptableObject
{
    private static int DEFAULT_ACTION_POINTS = 30;

    private static int DEFAULT_ACTION_POINTS_PER_TURN = 10;

    public int TurnCount { get; private set; }

    public int EnemyTurnCount { get; private set; }

    public int ActionPointReserve { get; private set; }

    public int ActionPointsPerTurn { get; private set; }

    public int ActionPointsUsedThisTurn { get; private set; }

    public List<Character> ActiveRoster { get; private set; }
    public List<Character> GoaledRoster { get; private set; }
    public List<Character> InActiveRoster { get; private set; }

    public Player()
    {
        TurnCount = 1;
        EnemyTurnCount = 0;
        ActionPointReserve = DEFAULT_ACTION_POINTS;
        ActionPointsPerTurn = DEFAULT_ACTION_POINTS_PER_TURN;
        ActionPointsUsedThisTurn = 0;

        ActiveRoster = new List<Character>();
        GoaledRoster  = new List<Character>();
        InActiveRoster = new List<Character>();
    }


    public void SpendActionPoint(int amount)
    {
        ActionPointsUsedThisTurn += amount;
        EventManager.TriggerEvent(EventNames.UI_ACTION_POINT_CONSUMED, this);
    }

    public void TurnIncrement()
    {
        TurnCount++;
        ActionPointReserve -= ActionPointsUsedThisTurn;
        EventManager.TriggerEvent(EventNames.UI_USER_END_TURN, this);
        EventManager.TriggerEvent(EventNames.UI_DEBIT_TOTAL_ACTION_POINTS, this);
    }

    public void EnemyTurnIncrement()
    {
        EnemyTurnCount++;
        ActionPointsUsedThisTurn = 0;
        EventManager.TriggerEvent(EventNames.UI_ENEMY_END_TURN, this);
    }

    public void ActivateCharacter(Character character)
    {
        ActiveRoster.Add(character);
        GoaledRoster.Remove(character);
        InActiveRoster.Remove(character);
        EventManager.TriggerEvent(EventNames.UI_CHARACTER_ACTIVATED, this);
    }

    public void GoalCharacter(Character character)
    {
        GoaledRoster.Add(character);
        ActiveRoster.Remove(character);
        InActiveRoster.Remove(character);
        EventManager.TriggerEvent(EventNames.UI_CHARACTER_GOALED, this);
    }

    public void DeactivateCharacter(Character character)
    {
        InActiveRoster.Add(character);
        ActiveRoster.Remove(character);
        GoaledRoster.Remove(character);
        EventManager.TriggerEvent(EventNames.UI_CHARACTER_DEACTIVATED, this);
    }
}
