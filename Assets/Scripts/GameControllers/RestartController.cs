using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RestartController : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("small level", LoadSceneMode.Single);
    }
}
