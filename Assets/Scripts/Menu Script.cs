using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void gotoCredits()
    {
        SceneManager.LoadScene("Credits Scene");
    }

    public void exitGame()
    {
        Debug.Log("Quitting the Game.");
        Application.Quit();
    }
}
