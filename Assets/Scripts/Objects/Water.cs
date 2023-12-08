using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject GameOverUI;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            // SceneManager.LoadScene("Main Scene");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0f;
            GameOverUI.SetActive(true);
        }
    }
}
