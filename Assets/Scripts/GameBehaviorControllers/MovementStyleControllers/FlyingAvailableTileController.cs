using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAvailableTileController : MonoBehaviour, MouseEventActionProvider
{
    public int flatCostOfFlight;

    public Action OnMouseDownAction()
    {
        return new NoAction();
    }

    public Action OnMouseEnterAction()
    {
        return new SingleBreadCrumbAction(transform.position, flatCostOfFlight);
    }
}
