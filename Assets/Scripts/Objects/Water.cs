using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Water : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            SceneManager.LoadScene("Main Scene");
        }
    }
    
}
