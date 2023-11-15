using UnityEngine;

public class WoodenPlankScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("DeadZone"))
        {
            Destroy(gameObject);
        }
    }
}
