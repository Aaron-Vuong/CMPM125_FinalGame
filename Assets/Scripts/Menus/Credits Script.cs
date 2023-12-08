using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public TextMeshProUGUI pageOne;
    public TextMeshProUGUI pageTwo;
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
            pageOne.enabled = false;
            pageTwo.enabled = true;
            currentPage++;
        }
    }

    public void prevPage()
    {
        if (currentPage == 2)
        {
            pageOne.enabled = true;
            pageTwo.enabled = false;
            currentPage--;
        }
    }

    public void back()
    {
        prevPage();
        SceneManager.LoadScene("Title Scene");
    }
}
