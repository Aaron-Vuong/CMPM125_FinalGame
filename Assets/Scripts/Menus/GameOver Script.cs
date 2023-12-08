using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject player;
    public void restartCheckpoint()
    {
        unpause();
        gameObject.SetActive(false);
        player.GetComponent<FirstPersonPlayerController> ().BackToCheckPoint();
    }

    public void restartLevel()
    {
        unpause();
        SceneManager.LoadScene("Main Scene");
    }

    public void backToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title Scene");
    }

    private void unpause()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
