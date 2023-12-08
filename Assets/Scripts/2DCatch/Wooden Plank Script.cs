using UnityEngine;

public class WoodenPlankScript : MonoBehaviour
{
    
    // [SerializeField] private AudioSource deletePlankSfx;
    [SerializeField] private ParticleSystem explosionParticles;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("DeadZone"))
        {
            GameObject.FindGameObjectWithTag("2DPlayer").GetComponent<MiniGamePlayerController>().playSoundEffect();
            // deletePlankSfx.PlayOneShot(deletePlankSfx.clip, 0.75f);
            Instantiate(explosionParticles, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
