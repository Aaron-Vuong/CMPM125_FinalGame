using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public AudioSource selectSfx;
    private float volume = 0.75f;
    public void playGame()
    {
        selectSfx.PlayOneShot(selectSfx.clip, volume);
        SceneManager.LoadScene("Main Scene");
    }

    public void gotoCredits()
    {
        selectSfx.PlayOneShot(selectSfx.clip, volume);
        SceneManager.LoadScene("Credits Scene");
    }

    public void exitGame()
    {
        selectSfx.PlayOneShot(selectSfx.clip, volume);
        Debug.Log("Quitting the Game.");
        Application.Quit();
    }
}
