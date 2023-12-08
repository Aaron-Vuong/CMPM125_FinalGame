using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    public string nextScene;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            SceneManager.LoadScene(nextScene);
        }
    }
}
