using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandheldGames
{
    None,
    Catch,
    Break
} 


public class Cartridge : MonoBehaviour
{
    [SerializeField]
    private HandheldGames gameToSelect;
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        Debug.Log(other.tag);
        // Set the game to display and disappear.
        if (other.tag == "Player") {
            FirstPersonPlayerController controller = other.GetComponent<FirstPersonPlayerController>();
            controller.currentSelectedGame = gameToSelect;
            gameObject.SetActive(false);
        }
    }
}
