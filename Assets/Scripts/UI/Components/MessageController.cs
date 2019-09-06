using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageController : MonoBehaviour
{

    Text textBox;

    private void Start()
    {
        textBox = GetComponent<Text>();

        EventManager.StartListening(EventNames.UI_CHARACTER_DEACTIVATED, OnCharacterDeactivated);
        EventManager.StartListening(EventNames.UI_CHARACTER_GOALED, OnCharacterGoaled);
    }

    private void OnCharacterGoaled(object argument)
    {
        var player = argument as Player;
        var character = player.GoaledRoster[player.GoaledRoster.Count - 1];
        ShowMessage(character.Name + " reached the Goal!");
    }

    private void OnCharacterDeactivated(object argument)
    {
        var player = argument as Player;
        var character = player.InActiveRoster[player.InActiveRoster.Count - 1];
        ShowMessage(character.name + " has fallen!");
    }

    private void ShowMessage(object message)
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
