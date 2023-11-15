using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class FirstPersonPlayerController : MonoBehaviour
{
    [Header("Ray Detection")] 
    [SerializeField] private Transform groundRayOriginTransform;
    [SerializeField] private float cameraRayLength;
    [SerializeField] private float groundRayLength;
    
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

    [Header("Camera Ray UI Text")] 
    [SerializeField] private TMP_Text cameraRayCheckText;
    
    //Private variables
    private float _xInput;
    private float _yInput;
    private bool _isGrounded;
    private LayerMask _onlyGroundLayerMask = 1 << 7;
    private LayerMask _onlyHandheld = 1 << 8;
    private LayerMask _ignorePlayerLayerMask = ~(1 << 6);

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
        InitiateUI();
    }

    private void Update()
    {
        GroundCheck();
        CameraRayCheck();
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
    
    private void InitiateUI()
    {
        if (cameraRayCheckText.enabled || cameraRayCheckText.text.Length > 0)
        {
            cameraRayCheckText.text = string.Empty;
            cameraRayCheckText.enabled = false;
        }
    }
    
    private void GroundCheck()
    {
        Ray downRay = new Ray(groundRayOriginTransform.position, Vector3.down);
        RaycastHit downRayHit;
        
        if (Physics.Raycast(downRay, out downRayHit, groundRayLength, _onlyGroundLayerMask))
        {
            Debug.DrawRay(downRay.origin, downRay.direction * groundRayLength, Color.green);
            if (!_isGrounded)
            {
                _isGrounded = true;
            }
        }
        else
        {
            Debug.DrawRay(downRay.origin, downRay.direction * groundRayLength, Color.red);
            if (_isGrounded)
            {
                _isGrounded = false;
            }
        }
    }
    
    private void CameraRayCheck()
    {
        Ray cameraRay = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, cameraRayLength, _onlyHandheld))
        {
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * cameraRayLength, Color.green);

            if (!cameraRayCheckText.enabled)
            {
                cameraRayCheckText.enabled = true;
            }

            cameraRayCheckText.text = "Interact With Handheld";
        }
        else
        {
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * cameraRayLength, Color.red);
            
            if (cameraRayCheckText.enabled)
            {
                cameraRayCheckText.text = String.Empty;
                cameraRayCheckText.enabled = false;
            }
        }
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
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
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
                //Debug.Log("Camera rotation on X is higher than max value");
                camTrans.localEulerAngles = new Vector3(angleXMax, 0, 0);
            }

            if (angleX < -angleXMax)
            {
                //Debug.Log("Camera rotation on X is lower than min value");
                camTrans.localEulerAngles = new Vector3(-angleXMax, 0, 0);
            }
        }
    }
}
