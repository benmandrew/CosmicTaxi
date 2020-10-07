using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void LoadGame ()
    {
        SceneManager.LoadScene("AlbaScene new");
    }

    public void QuitGame ()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
