using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerTextBoxController : MonoBehaviour
{
    public Text textBox;
    
    // Start is called before the first frame update
    void Start()
    {
        var parentId = GetComponent<PlayerCharacterController>().GetInstanceID();
        var eventName = string.Format(EventNames.CHARACTER_TEXT_DISPLAY, parentId);
        EventManager.StartListening(eventName, OnTextEventRecieved);    
    }

    private void OnTextEventRecieved(object arg0)
    {
        StartCoroutine(ShowMessage(arg0 as string));
    }

    IEnumerator ShowMessage(string message)
    {
        textBox.text = message;
        textBox.enabled = true;
        yield return new WaitForSeconds(3);
        textBox.enabled = false;
    }
}
