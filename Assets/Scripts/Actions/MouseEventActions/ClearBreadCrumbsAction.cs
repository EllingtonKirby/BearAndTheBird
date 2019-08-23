using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearBreadCrumbsAction : Action
{
    public IEnumerator Perform()
    {
        BreadCrumbController.instance.CheckAndPopFirstMove();
        yield return null;
    }
}
