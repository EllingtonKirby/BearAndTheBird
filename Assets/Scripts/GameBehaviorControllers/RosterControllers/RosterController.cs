using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RosterController : MonoBehaviour
{
    public static RosterController instance;

    private ArrayList activeRoster = new ArrayList();
    private int activeCharacters = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    public void AddCharacterToRoster(GameObject character)
    {
        activeRoster.Add(character);
        activeCharacters++;
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
         
        }
    }
}
