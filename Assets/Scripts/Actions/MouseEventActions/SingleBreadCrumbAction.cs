using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBreadCrumbAction : Action
{
    private readonly Vector3 position;
    private readonly int costOfFlight;

    public SingleBreadCrumbAction(Vector3 position, int costOfFlight)
    {
        this.position = position;
        this.costOfFlight = costOfFlight;
    }

    public IEnumerator Perform()
    {
        BreadCrumbController.instance.MoveSingleBreadCrumb(new BreadCrumb(position, costOfFlight));
        yield return null;
    }
}
