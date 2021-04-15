using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1_final");
    }

}
