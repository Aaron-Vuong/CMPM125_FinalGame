using System;
using TMPro;
using UnityEngine;

public class MiniGamePlayerController : MonoBehaviour
{
    [Header("Player Movement")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] [Range(0, 0.5f)] private float moveLerpFactor;
    [SerializeField] [Range(0, 0.5f)] private float stopLerpFactor;

    [Header("Spawn Point")] 
    [SerializeField] private Transform spawnPointTransform;

    [Header("Plank Count UI")] 
    [SerializeField] private TMP_Text plankCountText;
    
    //Private movement variables
    private float _xInput;

    private int _plankCount;
    
    //Private components
    private Rigidbody _rb;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        ReceiveMovementInput();
    }

    private void FixedUpdate()
    {
        ApplyMovementOnGround();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            transform.position = spawnPointTransform.position;
        }

        if (other.CompareTag("WoodenPlank"))
        {
            Destroy(other.gameObject);
            _plankCount++;
            plankCountText.text = "Wooden Plank: " + _plankCount;
        }
    }

    //------------------------------------------------Non-Update Area-------------------------------------------------------
    
    private void ReceiveMovementInput()
    {
        //If have movement input, store _xInput & _yInput
        if (Input.GetAxis("Horizontal") != 0)
        {
            _xInput = Input.GetAxis("Horizontal");
        }
        //If no input, clear saved value
        else
        {
            _xInput = 0;
        }
    }
    
    private void ApplyMovementOnGround()
    {
        if (_xInput != 0)
        {
            Vector3 tempVelocity = _rb.velocity;
            Vector3 targetVelocity = new Vector3(moveSpeed * _xInput, tempVelocity.y,0);

            _rb.velocity = Vector3.Lerp(tempVelocity, targetVelocity, moveLerpFactor);
        }
        else if (_rb.velocity != Vector3.zero)
        {
            Vector3 tempVelocity = _rb.velocity;
            _rb.velocity = Vector3.Lerp(tempVelocity, Vector3.zero, stopLerpFactor);
        }
    }
}
