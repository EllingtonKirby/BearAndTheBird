using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RosterController : MonoBehaviour
{
    public static RosterController instance;

    public GameObject[] availableCharacters;
    public int rosterSlots;

    private ArrayList activeRoster = new ArrayList();
    private int activeCharacters = 2;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        activeCharacters = availableCharacters.Length;
    }

    public void OnCharacterDeath(string characterName)
    {
        activeCharacters--;
        if (activeCharacters == 0)
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
        else
        {
            MessageController.instace.ShowMessage(characterName + " Died!");
        }
    }

    public void OnCharacterReachGoal(string characterName)
    {
        activeCharacters--;
        if (activeCharacters == 0)
        {
            SceneManager.LoadScene("Victory", LoadSceneMode.Single);
        }
        else
        {
            MessageController.instace.ShowMessage(characterName + " Reached the goal!");
        }
    }

    public void OnGridCompleted()
    {
        for (int i = 0; i < rosterSlots; i++)
        {
            var character = Instantiate(availableCharacters[i]);
            activeRoster.Add(character);
        }
    }
}
