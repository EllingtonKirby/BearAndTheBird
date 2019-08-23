using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAvailableTileController : MonoBehaviour, MouseEventActionProvider
{
    public int flatCostOfFlight;

    private void OnMouseEnter()
    {
        EventManager.TriggerMouseEvent(EventNames.MOUSE_ENTER, this);
    }

    private void OnMouseDown()
    {
        EventManager.TriggerMouseEvent(EventNames.MOUSE_DOWN, this);
    }

    public Action OnMouseDownAction()
    {
        return new NoAction();
    }

    public Action OnMouseEnterAction()
    {
        return new SingleBreadCrumbAction(transform.position, flatCostOfFlight);
    }
}
