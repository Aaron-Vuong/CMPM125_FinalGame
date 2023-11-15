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

    [Header("Win UI")] 
    [SerializeField] private TMP_Text winText;
    
    //Private movement variables
    private float _xInput;

    private int _plankCount;

    private bool _win;
    
    //Private components
    private Rigidbody _rb;

    //Singleton
    private static MiniGamePlayerController _instance;
    public static MiniGamePlayerController Instance
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
        _win = false;
    }
    
    private void Update()
    {
        //ReceiveMovementInput();
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

            if(_plankCount >= 5){ // win
                _win = true;
                plankCountText.enabled = false;
                winText.enabled = true;
                FirstPersonPlayerController.Instance.BuildBridge();
            }
        }
    }

    //------------------------------------------------Non-Update Area-------------------------------------------------------
    
    public void ReceiveMovementInput()
    {
        //If game not over and have movement input, store _xInput & _yInput
        if (!_win && Input.GetAxis("Horizontal") != 0)
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

    public bool getWin(){
        return _win;
    }
}
