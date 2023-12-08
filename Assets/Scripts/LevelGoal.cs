using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    public string nextScene;
    [SerializeField] private Animator crossFadeAnimator;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            //SceneManager.LoadScene(nextScene);
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        crossFadeAnimator.Play("Cross Fade Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextScene);
    }
}
