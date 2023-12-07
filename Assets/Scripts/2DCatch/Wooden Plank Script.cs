using UnityEngine;

public class WoodenPlankScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("DeadZone"))
        {
            Instantiate(explosionParticles, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
