using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventNames 
{
    public static string END_TURN = "end_turn";

    public static string START_MOVE = "start_move";
    public static string END_MOVE = "end_move";

    public static string MOUSE_ENTER = "mouse_enter";
    public static string MOUSE_DOWN = "mouse_down";

    public static string ADDED_TO_BREADCRUMB = "added_to_breadcrumb_{0}_{1}";
    public static string REMOVED_FROM_BREADCRUMB = "removed_from_breadcrumb_{0}_{1}";

    public static string MOVEMENT_QUEUED = "movement_queued";
    public static string MOVEMENT_REMOVED_FROM_QUEUE = "movement_removed_from_queue";

    public static string MOVED_TO_POSITION = "moved_to_position_{0}_{1}";
    public static string MOVEMENT_CONSUMED = "movement_consumed";

    public static string ENEMY_TURN_START = "enemy_turn_start";
    public static string ENEMY_ACTION_COMPLETED = "enemy_action_completed";
    public static string ENEMY_TURN_COMPLETED = "enemy_turn_completed";

    public static string CHARACTER_TEXT_DISPLAY = "character_text_display_{0}";

    public static string SHOW_MESSAGE = "show_message";

    public static string TERMINATE_MOVE = "terminate_move";
}
