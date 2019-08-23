using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCrumbTileController : MonoBehaviour, MouseEventActionProvider
{

    private void Start()
    {
        var eventNameRemoved = string.Format(EventNames.MOVED_TO_POSITION, transform.position.x, transform.position.y);
        EventManager.StartListening(eventNameRemoved, delegate { Destroy(this.gameObject); });
    }

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
        return new MoveToPositionAction();
    }

    public Action OnMouseEnterAction()
    {
        return new NoAction();
    }
}
