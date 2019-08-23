using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StatefulActor
{
    State GetState();
    void SetIdle();
    void SetSelectedForMovement();
}

public enum State
{
    IDLE = 0, SELECTED_FOR_MOVEMENT, MOVE_IN_PROGRESS
}
