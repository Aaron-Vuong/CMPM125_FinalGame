using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FirstPersonPlayerController.Instance.currentCheckPoint = transform;
        }
    }
}
