using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BallBounceTest : MonoBehaviour
{
    private int speed = 10;
    private Vector3 direction;
    private float volume = 0.75f;
    [SerializeField] private AudioSource hitBlockSfx;
    [SerializeField] private AudioSource ballBounceSfx;

    [SerializeField] private ParticleSystem explosionParticles;


    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.down; //new Vector3(Random.Range(-0.5f, 0.5f),-1,0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("2DPlayer"))
        {
            if (collision.rigidbody != null)
            {
                // Reflect in the direction that we are moving and keep the same magnitude.
                Vector3 new_direction = direction.normalized + collision.rigidbody.velocity.normalized;
                direction = new_direction.normalized * direction.magnitude;
            }
        }
        // Apply the reflection force to the ball
        ballBounceSfx.PlayOneShot(ballBounceSfx.clip, volume);
        direction = Vector3.Reflect(direction, collision.GetContact(0).normal);
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("block"))
        {
            Instantiate(explosionParticles, collision.gameObject.transform.position, Quaternion.identity);
            MiniGamePlayerController.Instance.blocks.Remove(collision.gameObject);
            Destroy(collision.gameObject, 0.1f);
            hitBlockSfx.PlayOneShot(hitBlockSfx.clip, volume);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            loseLife();
        }
    }

    private void loseLife()
    {
        Debug.Log("Life Lost");
        transform.gameObject.SetActive(false);
    }
}
