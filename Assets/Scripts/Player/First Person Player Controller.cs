using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonPlayerController : MonoBehaviour
{
    [Header("Ray Detection")] 
    [SerializeField] private Transform groundRayOriginTransform;
    [SerializeField] private float cameraRayLength;
    [SerializeField] private float groundRayLength;
    
    [Header("Player Movement")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0, 0.5f)] private float moveLerpFactor;
    [SerializeField] [Range(0, 0.5f)] private float stopLerpFactor;

    [Header("First Person Camera")] 
    [SerializeField] private Transform camTrans;

    [Header("Camera Movement")] 
    [SerializeField] private float cameraHorizontalSpeed;
    [SerializeField] private float cameraVerticalSpeed;
    [SerializeField] [Range(30, 80)] private float angleXMax;

    [Header("Camera Ray UI Text")] 
    [SerializeField] private TMP_Text cameraRayCheckText;

    [Header("Bridges & Breakables")] 
    [SerializeField] private float minimumDistance;
    [SerializeField] private Material bridgeMaterial;
    [SerializeField] private ParticleSystem explosionParticles;
    private GameObject[] bridges;
    private GameObject closestBridge;
    private List<GameObject> breakables;
    private GameObject closestBreakable;

    [Header("Check Point")] 
    public Transform currentCheckPoint;

    

    [Header("Trail Object")]
    [SerializeField] private GameObject trail;
    private float trailSpeed = 25f;
    

    [Header("Game Selection")]
    public HandheldGames currentSelectedGame = HandheldGames.None;

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
        bridges = GameObject.FindGameObjectsWithTag("Bridge");
        GameObject[] existingBreakables = GameObject.FindGameObjectsWithTag("Breakable");
        Debug.Log(existingBreakables.Length);

        breakables = new List<GameObject>();

        foreach (GameObject b in existingBreakables) {
            breakables.Add(b);
        }

        Debug.Log(breakables.Count);
        SetClosestBridge();
        SetClosestBreakable();
        InitiateMouse();
        InitiateUI();
    }

    private void Update()
    {
        GroundCheck();
        ClickCheck();
        if (trail && currentSelectedGame == HandheldGames.Catch) {
            trail.transform.position = Vector3.MoveTowards(trail.transform.position, closestBridge.transform.position, trailSpeed * Time.deltaTime);
        } else if (trail && currentSelectedGame == HandheldGames.Break && breakables.Count != 0) {
            trail.transform.position = Vector3.MoveTowards(trail.transform.position, closestBreakable.transform.position, trailSpeed * Time.deltaTime);
        }
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

    private void ApplyJump() {
        _rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
    }
    
    private void ClickCheck() {
        if (Input.GetButtonDown("Fire1")) {
            CameraRayCheck();

            switch(currentSelectedGame) {
                case HandheldGames.None:
                    break;
                case HandheldGames.Catch:
                    trail.transform.position = camTrans.position + (camTrans.forward * 5f);
                    SetClosestBridge();
                    break;
                case HandheldGames.Break:
                    trail.transform.position = camTrans.position + (camTrans.forward * 5f);
                    SetClosestBreakable();
                    break;
            }
        }
    }

    private void CameraRayCheck()
    {
        if (currentSelectedGame == HandheldGames.None) { return; }
        Ray cameraRay = new Ray(camTrans.position, camTrans.forward);
        RaycastHit hit;

        if (Physics.Raycast(cameraRay, out hit, cameraRayLength, _onlyHandheld))
        {
            Debug.DrawRay(cameraRay.origin, cameraRay.direction * cameraRayLength, Color.green);

            if (!cameraRayCheckText.enabled)
            {
                cameraRayCheckText.enabled = true;
                Debug.Log("Looking down");
                // https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
                
                switch (currentSelectedGame){
                    case HandheldGames.Catch:
                        SceneManager.LoadScene("SupplyCatcher", LoadSceneMode.Additive);
                        break;
                    
                    case HandheldGames.Break:
                        SceneManager.LoadScene("BreakOut", LoadSceneMode.Additive);
                        break;
                }
                
                ControllerManager.Instance.setControllerState(ControllerManager.ControllerStates._2DGame);

                // stop player
                _xInput = 0;
                _yInput = 0;

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
                Debug.Log("not looking down");
                ControllerManager.Instance.setControllerState(ControllerManager.ControllerStates._3DFPGame);
                //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.UnloadSceneAsync.html
                
                switch (currentSelectedGame){
                    case HandheldGames.Catch:
                        SceneManager.UnloadSceneAsync("SupplyCatcher");
                        break;
                    
                    case HandheldGames.Break:
                        SceneManager.UnloadSceneAsync("BreakOut");
                        break;
                }

                
            }
        }
    }
    
    public void ReceiveMovementInput()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded) {
            Debug.Log("jump");
            ApplyJump();
        }
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

    private void SetClosestBridge() {
        float minDist = minimumDistance;
        foreach(GameObject b in bridges) {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < minDist && !b.GetComponent<BoxCollider>().enabled) {
                closestBridge = b;
                minDist = dist;
            }
        }
    }
    public void BuildBridge(){
        if (!closestBridge.GetComponent<BoxCollider>().enabled) {
            closestBridge.GetComponent<BoxCollider>().enabled = true;
            closestBridge.GetComponent<Renderer>().material = bridgeMaterial;
            SetClosestBridge();
        }
    }

    private void SetClosestBreakable() {
        float minDist = minimumDistance;
        foreach (GameObject b in breakables) {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < minDist) {
                closestBreakable = b;
                minDist = dist;
            }
        }
    }
    public void Break() {
        Instantiate(explosionParticles, closestBreakable.transform.position, Quaternion.identity);
        breakables.Remove(closestBreakable);
        Destroy(closestBreakable);
    }

    public void BackToCheckPoint()
    {
        transform.position = currentCheckPoint.position;
    }
}
