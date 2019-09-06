using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTurnButtonController : MonoBehaviour
{

    private Button button;
    private Text label;
    

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        label = GetComponent<Text>();
        button.onClick.AddListener(OnButtonClick);

        EventManager.StartListening(EventNames.UI_USER_END_TURN, OnPlayerTurnEnd);
        EventManager.StartListening(EventNames.UI_ENEMY_END_TURN, OnEnemyTurnEnd);
    }

    public Button.ButtonClickedEvent GetOnNextTurnClicked()
    {
        return button.onClick;
    }

    private void OnButtonClick()
    {
        LevelStoreController.instance.GetPlayer().TurnIncrement();
    }

    private void OnPlayerTurnEnd(object argument)
    {
        button.interactable = false;
        label.text = "Enemy Turn";
    }

    private void OnEnemyTurnEnd(object argument)
    {
        button.interactable = true;
        label.text = "Next Turn";
    }
}
