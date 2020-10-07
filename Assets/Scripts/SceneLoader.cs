using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("BeanScene");
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
}
