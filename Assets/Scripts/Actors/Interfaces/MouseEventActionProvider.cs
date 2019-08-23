using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MouseEventActionProvider
{
    Action OnMouseDownAction();
    Action OnMouseEnterAction();
}

public interface Action
{
    IEnumerator Perform();
}
