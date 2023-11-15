using System;
using Unity.Mathematics;
using UnityEngine;

public class FirstPersonPlayerController : MonoBehaviour
{
    [Header("Player Movement")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] [Range(0, 0.5f)] private float moveLerpFactor;
    [SerializeField] [Range(0, 0.5f)] private float stopLerpFactor;

    [Header("First Person Camera Transform")] 
    [SerializeField] private Transform camTrans;

    [Header("Camera Movement")] 
    [SerializeField] private float cameraHorizontalSpeed;
    [SerializeField] private float cameraVerticalSpeed;
    [SerializeField] [Range(30, 80)] private float angleXMax;
    
    //Private variables
    private float _xInput;
    private float _yInput;
    
    //Private components
    private Rigidbody _rb;
    
    //Singleton
    private static FirstPersonPlayerController _instance;
    public static FirstPersonPlayerController Instance
    {
        get
        {
            return _instance;
        }
    }
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        InitiateMouse();
    }

    private void Update()
    {
        ReceiveMovementInput();
    }

    private void FixedUpdate()
    {
        MouseMovement();
        ApplyMovementOnGround();
    }

    //------------------------------------------------Non-Update Area-------------------------------------------------------

    private void InitiateMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void ReceiveMovementInput()
    {
        //If have movement input, store _xInput & _yInput
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            _xInput = Input.GetAxis("Horizontal");
            _yInput = Input.GetAxis("Vertical");
        }
        //If no input, clear saved value
        else
        {
            _xInput = 0;
            _yInput = 0;
        }
    }

    private void ApplyMovementOnGround()
    {
        if (_xInput != 0 || _yInput != 0)
        {
            Vector3 directionVector = Vector3.Normalize(new Vector3(transform.forward.x, 0, transform.forward.z));
            float inputRotateAngle = Vector3.SignedAngle(Vector3.forward, new Vector3(_xInput, 0, _yInput), Vector3.up);
            Quaternion inputRotation = Quaternion.Euler(0, inputRotateAngle, 0);
            
            Vector3 tempVelocity = _rb.velocity;
            Vector3 targetVelocity = inputRotation * new Vector3(moveSpeed * directionVector.x, tempVelocity.y,moveSpeed * directionVector.z);

            _rb.velocity = Vector3.Lerp(tempVelocity, targetVelocity, moveLerpFactor);
        }
        else if (_rb.velocity != Vector3.zero)
        {
            Vector3 tempVelocity = _rb.velocity;
            _rb.velocity = Vector3.Lerp(tempVelocity, Vector3.zero, stopLerpFactor);
        }
    }

    private void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        Quaternion quaternionX = quaternion.Euler(new Vector3(0, mouseX * cameraHorizontalSpeed * Time.fixedDeltaTime, 0));
        Quaternion quaternionY = quaternion.Euler(new float3(-mouseY * cameraVerticalSpeed * Time.fixedDeltaTime, 0, 0));

        transform.rotation = quaternionX * transform.rotation;
        camTrans.rotation = camTrans.rotation * quaternionY;

        float angleX = camTrans.localEulerAngles.x;
        
        if (angleX > 180)
        {
            angleX -= 360;
        }

        if (angleX < -180)
        {
            angleX += 360;
        }
        
        if (angleX > angleXMax)
        {
            //Debug.Log(camTrans.eulerAngles);
            Debug.Log("Camera rotation on X is higher than max value");
            camTrans.localEulerAngles = new Vector3(angleXMax, 0, 0);
        }

        if (angleX < -angleXMax)
        {
            Debug.Log("Camera rotation on X is lower than min value");
            camTrans.localEulerAngles = new Vector3(-angleXMax, 0, 0);
        }
    }
}
