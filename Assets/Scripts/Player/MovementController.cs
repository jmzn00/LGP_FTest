using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{

    [Header("User Settings")]
    [SerializeField] private float mouseSensitivity = 1f;
    [Space]
    [Header("Settings")]
    [SerializeField] private float groundAcceleration = 100f;
    [SerializeField] private float groundLimit = 12f;
    [SerializeField] private float friction = 6f;
    [SerializeField] private float slopeLimit = 60f;
    [Space]
    [Header("Crouch")]
    [SerializeField] private GameObject playerVisuals;
    [SerializeField] private CapsuleCollider playerCollider;
    [Space]
    [Header("Jump")]
    [SerializeField] private bool additiveJump = true;
    [SerializeField] private bool autoJump = true;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravity = 16f;
    private bool jumpPending;
    private bool canJump = true;
    [Space]
    [Header("AirMovement")]
    [SerializeField] private float airLimit = 1f;
    [SerializeField] private float airAcceleration = 100f;
    [Header("Camera")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 1.75f, 0);
    [Space]
    [SerializeField] private float thirdPersonCameraDistance = 4f;
    [SerializeField] private float thirdPersonCameraHeight = 5f;
    [SerializeField] private float thirdPersonCameraLookAtHeight = 3f;
    private CameraController _cameraController;
    private bool isThirdPersonCamera = false;
    private bool overrideCamera = false;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    public Vector2 LookInput => _lookInput;

    private Vector3 _velocity;
    private Vector3 _inputDir;
    private Vector3 _inputRot;
    public Vector3 InputRot => _inputRot;

    private Rigidbody _rb;

    private Vector3 groundNormal;
    private bool _isGrounded;

    private MovingPlatform _currentPlatform;
    private Vector3 _lastPlatformPosition;
    

    private void Start()
    {
        SubscribeInputs();

        _rb = GetComponent<Rigidbody>();
        _cameraController = GetComponent<CameraController>();
    }

    #region Inputs
    private void SubscribeInputs()
    {
        if(InputManager.Instance == null) 
        {
            Debug.LogError("InputManager is NULL");
            return;
        }
        InputManager.Instance.Actions.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        InputManager.Instance.Actions.Player.Move.canceled += ctx => _moveInput = Vector2.zero;

        InputManager.Instance.Actions.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        InputManager.Instance.Actions.Player.Look.canceled += ctx => _lookInput = Vector3.zero;

        InputManager.Instance.Actions.Player.Jump.performed += ctx => jumpPending = true;
        InputManager.Instance.Actions.Player.Jump.canceled += ctx => jumpPending = false;

        InputManager.Instance.Actions.Player.ToggleThirdPerson.performed += ctx => isThirdPersonCamera = !isThirdPersonCamera;
        //InputManager.Instance.Actions.Player.ToggleThirdPerson.canceled += ctx => isThirdPersonCamera = false;

        //InputManager.Instance.Actions.Player.Sprint.performed += ctx => dashPending = true;

        InputManager.Instance.Actions.Player.Crouch.performed += ctx => Crouch(true);
        InputManager.Instance.Actions.Player.Crouch.canceled += ctx => Crouch(false);

    }
    private void GetMovementInput()
    {
        float x = _moveInput.x;
        float z = _moveInput.y;

        _inputDir = transform.rotation * new Vector3(x, 0, z).normalized;
    }
    private void GetMouseInput()
    { 
        _inputRot.y += _lookInput.x * mouseSensitivity;
        _inputRot.x -= _lookInput.y * mouseSensitivity;

        if (_inputRot.x > 90f)
            _inputRot.x = 90f;
        if (_inputRot.x < -90f)
            _inputRot.x = -90f;
    }
    #endregion
    #region Camera
    public void SetCameraOverride(bool value)
    {
        overrideCamera = value;
    }
    private void CameraFollow()
    {
        if (!overrideCamera)
        {
            if (isThirdPersonCamera)
            {
                Vector3 cameraPos = transform.position - transform.forward
                    * thirdPersonCameraDistance + Vector3.up * thirdPersonCameraHeight;

                _cameraController.SetCameraPosition(cameraPos);
                _cameraController.SetCameraLookAt(transform.position + Vector3.up * thirdPersonCameraLookAtHeight);
            }
            else
            {
                _cameraController.SetCameraPosition(transform.position + cameraOffset);
                _cameraController.SetCameraRotation(Quaternion.Euler(_inputRot.x, _inputRot.y, 0f));
            }
        }

    }
    #endregion


    private void Update()
    {
        GetMovementInput();
        GetMouseInput();
    }

    private void FixedUpdate()
    {
        _velocity = _rb.linearVelocity;
        if (_isGrounded) 
        {
            _inputDir = Vector3.Cross(Vector3.Cross(groundNormal, _inputDir), groundNormal);
            GroundAccelerate();
            ApplyFriction();

            if(_currentPlatform != null) 
            {
                Vector3 platformDelta = _currentPlatform.transform.position - _lastPlatformPosition;
                _rb.position += platformDelta;
                _lastPlatformPosition = _currentPlatform.transform.position;
            }
            if (jumpPending)
            {
                Jump();
            }
        }
        else if(!_isGrounded)
        {
            AirAccelerate();
            ApplyGravity();
        }
        CameraFollow();
        transform.rotation = Quaternion.Euler(0f, _inputRot.y, 0f);

        _rb.linearVelocity = _velocity;
        _isGrounded = false;
        groundNormal = Vector3.zero;
    }

    private void Crouch(bool val) 
    {
        if (val) 
        {
            playerVisuals.transform.localScale = new Vector3(1f, 0.5f, 1f);            
        }
        else 
        {
            playerVisuals.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    private void Jump() 
    {
        if (!canJump) return;

        if (_velocity.y < 0f || !additiveJump)
            _velocity.y = 0f;

        _velocity.y += jumpHeight;
        _isGrounded = false;

        if (!autoJump)
            jumpPending = false;

        StartCoroutine(JumpTimer());
    }
    private IEnumerator JumpTimer() 
    {
        canJump = false;
        yield return new WaitForSeconds(0.1f);
        canJump = true;
    }

    private void GroundAccelerate()
    {
        float addSpeed = groundLimit - Vector3.Dot(_velocity, _inputDir);

        if (addSpeed <= 0)
            return; 

        float accelSpeed = groundAcceleration * Time.deltaTime;

        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;        
        _velocity += accelSpeed * _inputDir;
        
    }
    private void AirAccelerate() 
    {
        Vector3 hVel = _velocity;
        hVel.y = 0;

        float dot = Vector3.Dot(hVel, _inputDir);
        float addSpeed = airLimit - dot;

        if (addSpeed <= 0)
            return;

        float accelSpeed = airAcceleration * Time.deltaTime;

        if (accelSpeed > addSpeed)
            accelSpeed = addSpeed;

        _velocity += accelSpeed * _inputDir;
    }
    private void ApplyFriction() 
    {
        _velocity *= Mathf.Clamp01(1 - Time.deltaTime * friction);
    }
    private void ApplyGravity()
    {
        _velocity.y -= gravity * Time.deltaTime;
    } 
    private void OnCollisionStay(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts) 
        {
            if (contact.normal.y > Mathf.Sin(slopeLimit * (Mathf.PI / 180) + Mathf.PI / 2f))
            {
                groundNormal = contact.normal;
                _isGrounded = true;
             
                MovingPlatform platform = contact.otherCollider.gameObject.GetComponent<MovingPlatform>();
                if(platform != null) 
                {
                    if(_currentPlatform != platform) 
                    {
                        _currentPlatform = platform;
                        _lastPlatformPosition = platform.transform.position;
                    }
                }
                else 
                {
                    _currentPlatform = null;
                }
                return;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(_currentPlatform != null && collision.gameObject == _currentPlatform) 
        {
            _currentPlatform = null;
        }
    }
}
