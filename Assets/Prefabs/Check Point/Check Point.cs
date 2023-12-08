using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("in");
        if (other.CompareTag("Player"))
        {
            Debug.Log("setting");
            FirstPersonPlayerController.Instance.currentCheckPoint = transform;
        }
    }
}
