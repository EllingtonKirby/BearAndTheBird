using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageController : MonoBehaviour
{
    public static MessageController instace;
    public Text textBox;

    private void Awake()
    {
        if (instace == null)
        {
            instace = this;
        }
    }

    public void ShowMessage(object message)
    {
        StartCoroutine(ShowMessage(message as string));
    }

    IEnumerator ShowMessage(string message)
    {
        textBox.text = message;
        textBox.enabled = true;
        yield return new WaitForSeconds(3);
        textBox.enabled = false;
    }

}
