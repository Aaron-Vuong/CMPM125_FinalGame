using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{
    private float bounceMultiply = 15.0f;
    Rigidbody rb;

    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private float generationAreaWidthX;
    [SerializeField] private float generationAreaWidthY;
    private float Y_OFFSET = 9;
    [SerializeField] private GameObject blockObj;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
        int numBlocks = MiniGamePlayerController.Instance.blockGoal;
        for (int i = 0; i < numBlocks; i++) {
            float posX = Random.Range(-generationAreaWidthX, generationAreaWidthX);
            float posY = Y_OFFSET + Random.Range(-generationAreaWidthY, generationAreaWidthY);
            Vector3 generationPos = new Vector3(posX, posY, 0);

            Instantiate(blockObj, generationPos, Quaternion.identity);
        }
        MiniGamePlayerController.Instance.finishBreakoutSetup();

        float xForce = Random.Range(-5,5);
        rb.AddForce(new Vector3(xForce, -5, 0), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {

        // Apply the reflection force to the ball
        rb.velocity = Vector3.Reflect(rb.velocity, collision.contacts[0].normal).normalized * bounceMultiply;
        // Debug.Log(rb.velocity);
        if(rb.velocity.y < 0.02 && rb.velocity.y > -0.02){
            Vector3 temp = rb.velocity;
            
            if( rb.velocity.y < 0) // negative
                temp.y = -10;
            else
                temp.y = 10;

            rb.velocity = temp;
        }

        if(rb.velocity.x < 0.02 && rb.velocity.x > -0.02){
            Vector3 temp = rb.velocity;

            if( rb.velocity.x < 0) // negative
                temp.x = -10;
            else
                temp.x = 10;

            rb.velocity = temp;
        }


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