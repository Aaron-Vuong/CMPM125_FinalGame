using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    private float bounceMultiply = 25.0f;
    Rigidbody rb;

    [SerializeField] private ParticleSystem explosionParticles;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
        float xForce = Random.Range(-5,5);
        rb.AddForce(new Vector3(xForce, -5, 0), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        // Apply the reflection force to the ball
        Vector3 direction = Vector3.Reflect(rb.velocity.normalized, collision.GetContact(0).normal);
        
        rb.velocity = direction * Mathf.Max(bounceMultiply, rb.velocity.magnitude);
        Debug.Log(rb.velocity);
        // if(rb.velocity.y < 0.02 && rb.velocity.y > -0.02){
        //     Vector3 temp = rb.velocity;
            
        //     if( rb.velocity.y < 0) // negative
        //         temp.y = -10;
        //     else
        //         temp.y = 10;

        //     rb.velocity = temp;
        // }

        // if(rb.velocity.x < 0.02 && rb.velocity.x > -0.02){
        //     Vector3 temp = rb.velocity;

        //     if( rb.velocity.x < 0) // negative
        //         temp.x = -10;
        //     else
        //         temp.x = 10;

        //     rb.velocity = temp;
        // }


    }

    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.CompareTag("block"))
        {
            Instantiate(explosionParticles, collision.gameObject.transform.position, Quaternion.identity);
            MiniGamePlayerController.Instance.blocks.Remove(collision.gameObject);
            Destroy(collision.gameObject, 0.1f);
        }
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            loseLife();
        }
    }

    private void loseLife(){
        Debug.Log("Life Lost");
        transform.gameObject.SetActive(false);
    }
}