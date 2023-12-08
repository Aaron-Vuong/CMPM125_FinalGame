using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public TextMeshProUGUI pageOne;
    public TextMeshProUGUI pageTwo;
    public AudioSource selectSfx;
    private float volume = 0.75f;
    int currentPage = 1;
    void Start()
    {
        pageOne.enabled = true;
        pageTwo.enabled = false;
    }
    public void nextPage()
    {
        if(currentPage == 1)
        {
            selectSfx.PlayOneShot(selectSfx.clip, volume);
            pageOne.enabled = false;
            pageTwo.enabled = true;
            currentPage++;
        }
    }

    public void prevPage()
    {
        if (currentPage == 2)
        {
            selectSfx.PlayOneShot(selectSfx.clip, volume);
            pageOne.enabled = true;
            pageTwo.enabled = false;
            currentPage--;
        }
    }

    public void back()
    {
        selectSfx.PlayOneShot(selectSfx.clip, volume);
        prevPage();
        SceneManager.LoadScene("Title Scene");
    }
}
