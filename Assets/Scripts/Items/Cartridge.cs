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
        // Debug.Log("Triggered");
        // Debug.Log(other.tag);
        // Set the game to display and disappear.
        if (other.tag == "Player") {
            FirstPersonPlayerController controller = other.GetComponent<FirstPersonPlayerController>();
            controller.isHandheldEnabled = true;
            controller.collectCartridgeSfx.PlayOneShot(controller.collectCartridgeSfx.clip, controller.volume);
            controller.currentSelectedGame = gameToSelect;
            controller.gameInventory.Add(gameToSelect);
            gameObject.SetActive(false);
        }
    }
}
