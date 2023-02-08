using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    CHANGING_GRAVITY,
    DEFAULT,
}

public class MovementController : MonoBehaviour
{
    [SerializeField] private PlayerInputs _inputs;

    // Walk And Run variables ---------------------------------------------------------
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 5f;

    private Vector2 _previousYVelocity;

    // Jump variables -----------------------------------------------------------------
    [SerializeField] private JumpController _jumpController;
    private Vector2 _appliedGravity = Vector2.zero;
    private bool _isJumping = false;
    private bool _isGrounded;
    private bool _isFalling = false;
    
    [SerializeField] private float _coyoteTime = 0.2f;
    [SerializeField] private float _jumpBufferTime = 0.2f;
    private float _coyoteTimeCounter;
    private float _jumpBufferCounter;

    // Rigid Body variables -----------------------------------------------------------
    private Rigidbody2D _rb;
    
    private Vector2 _gravityVelocity;
    private Vector2 _movementVelocity;

    // Box Collider variables ---------------------------------------------------------
    private BoxCollider2D _boxCollider;

    // Other variables ----------------------------------------------------------------
    [SerializeField] private float _boxCastDistance = 0.05f;

    [SerializeField] private LayerMask _plateformLayerMask;
    
    private bool _isOnAPlanetTrigger = false;
    private bool _isCollidingAPlanet = false;

    private PlayerState _state = PlayerState.DEFAULT;
    private bool _canJump = true;

    [SerializeField] private float _jumpButtonGracePeriod = 0.2f;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    // Getters and Setters ------------------------------------------------------------
    public Vector2 MovementVelocity
    {
        get => _movementVelocity;
        set => _movementVelocity = value;
    }

    public Vector2 GravityVelocity
    {
        get => _gravityVelocity;
        set => _gravityVelocity = value;
    }

    public bool IsJumping {get { return _isJumping; }}

    public bool IsFalling => _isFalling;

    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

    public Rigidbody2D RigidBody2D { get { return _rb; } }

    public bool IsOnAPlanetTrigger { get => _isOnAPlanetTrigger; set => _isOnAPlanetTrigger = value; }

    public bool IsCollidingAPlanet { get => _isCollidingAPlanet; set => _isCollidingAPlanet = value; }

    public PlayerState State { get => _state; set => _state = value; }
    // --------------------------------------------------------------------------------
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Awake()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        // Methods to get inputs and do actions.
        // It's on the update in order to get as fast as possible the player's input and to response
        // to them as fast as possible.

        if (!_isOnAPlanetTrigger)
        {
            _isGrounded = CheckGroundedState();
        }
        else
        {
            _isGrounded = _isCollidingAPlanet;
        }

        if (_isGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        
        // if (_isGrounded)
        // {
        //     lastGroundedTime = Time.time;
        // }
        //
        // if (Input.GetButtonDown("Jump"))
        // {
        //     jumpButtonPressedTime = Time.time;
        // }
        
        HandleMoveHorizontal();
        HandleGravity();
        HandleJump();
        
    }

    private void FixedUpdate()
    {
        // Update the physic of the game with the input of the update.
        
        _rb.velocity = _gravityVelocity + _movementVelocity;
        

        // if (_isGrounded)
        // {
        //     _coyoteTimeCounter = _coyoteTime;
        // }
        // else
        // {
        //     _coyoteTimeCounter -= Time.deltaTime;
        // }
        //
        // if (_inputs.Jump)
        // {
        //     _jumpBufferCounter = _jumpBufferTime;
        // }
        // else
        // {
        //     _jumpBufferCounter -= Time.deltaTime;
        // }
    }
    
    private void HandleMoveHorizontal()
    {
        float targetSpeed;

        //Si Animation dans changement de gravité.
        if (_state == PlayerState.DEFAULT)
        {
            targetSpeed = _inputs.Run ? _runSpeed : _walkSpeed;
        }
        else
        {
            targetSpeed = 0f;
        }
        
        // Don't need to apply Time.deltaTime because the velocity of the rigid body already do it.
        
        if (_inputs.enabled)
        {
            // Horizontal Movements for the Vector of movement
            if (_jumpController.HorizontalSense.y == 0)
            {
                //_movementVelocity = _jumpController.HorizontalSense * (_inputs.Move.x * targetSpeed);
                _movementVelocity = transform.right * (_inputs.Move.x * targetSpeed);
            }
            // Vertical Movements for the Vector of movement
            else if (_jumpController.HorizontalSense.y != 0)
            {
                //_movementVelocity = _jumpController.HorizontalSense * (_inputs.Move.y * targetSpeed);
                _movementVelocity = transform.right * _jumpController.HorizontalSense * 
                                    (_inputs.Move.y * targetSpeed);
            }
        }
        else
        {
            _movementVelocity = Vector2.zero;
        }
    }
    
    private void HandleGravity()
    {
        _isFalling = Vector2.Dot(_gravityVelocity, _jumpController.BaseGravity) > 0;
        
        if (_isGrounded && !_isJumping)
        {
            _gravityVelocity = Vector2.zero;
            _isFalling = false;
        }
        // Correction of the problem when the player hold the jump button and stack some gravity velocity.
        else if (_isGrounded && _isFalling) 
        {
            _gravityVelocity = Vector2.zero;
            _isFalling = false;
        }

        if (State == PlayerState.DEFAULT)
        {
            if (Mathf.Abs(_gravityVelocity.x) <= 20f && Mathf.Abs(_gravityVelocity.y) <= 20f)
            {
                if (_isJumping)
                {
                    if (_isFalling || !_inputs.Jump)
                    {
                        _appliedGravity = _jumpController.JumpGravity * _jumpController.FallMultiplier;
                    }
                    else
                    {
                        _appliedGravity = _jumpController.JumpGravity;
                    }
                }
            
                else
                {
                    _appliedGravity = _jumpController.BaseGravity;
                }

                Vector2 newYVelocity = _appliedGravity * Time.deltaTime;
                Vector2 verletVelocity = (newYVelocity + _previousYVelocity) * 0.5f;
                _previousYVelocity = newYVelocity;
            
                _gravityVelocity += verletVelocity;
            }
        }
        
        else if (_state == PlayerState.CHANGING_GRAVITY)
        {
            if (Mathf.Abs(_gravityVelocity.x) >= 10f || Mathf.Abs(_gravityVelocity.y) >= 10f)
            {
                _gravityVelocity -= _gravityVelocity / 10;
            }

            if (Mathf.Abs(_gravityVelocity.x) <= 20f && Mathf.Abs(_gravityVelocity.y) <= 20f)
            {
                _gravityVelocity += _jumpController.BaseGravity * Time.deltaTime;
            }
            
            



            // if (Mathf.Abs(_gravityVelocity.x) <= 10f && Mathf.Abs(_gravityVelocity.y) <= 10f)
            // {
            //     _appliedGravity = _jumpController.BaseGravity;
            //     
            //     Vector2 newYVelocity = _appliedGravity * Time.deltaTime;
            //     Vector2 verletVelocity = (newYVelocity + _previousYVelocity) * 0.5f;
            //     _previousYVelocity = newYVelocity;
            //
            //     _gravityVelocity += verletVelocity;
            // }
        }



    }

    // private void HandleFallingState()
    // {
    //     if (!_gravityController.HasGravityChanged)
    //     {
    //         _isFalling = _rb.velocity.y <= 0f;
    //     }
    //
    //     else
    //     {
    //         _isFalling = _rb.velocity.y >= 0f;
    //     }
    // }
    
    

    private void HandleJump()
    {
        // if (Time.time - lastGroundedTime <= _jumpButtonGracePeriod)
        // {
        //     _isJumping = false;
        //     
        //     if (Time.time - jumpButtonPressedTime <= _jumpButtonGracePeriod)
        //     {
        //         _gravityVelocity = _jumpController.InitialJumpVelocity;
        //         _isJumping = true;        
        //         jumpButtonPressedTime = null;
        //         lastGroundedTime = null;
        //     }
        // }
        
        // if (_coyoteTimeCounter > 0f && _jumpBufferCounter > 0f && !_isJumping)
        // {
        //     _gravityVelocity = _jumpController.InitialJumpVelocity;
        //     _isJumping = true;
        //     _jumpBufferCounter = 0f;
        // }
        // else if (Input.GetButtonDown("Jump") && _isGrounded && _isJumping)
        // {
        //     _isJumping = false;
        //     _coyoteTimeCounter = 0f;
        // }
        
        if (_inputs.Jump && _coyoteTimeCounter > 0f && !_isJumping)
        {
            _gravityVelocity = _jumpController.InitialJumpVelocity;
            _isJumping = true;
        }
        else if (!_inputs.Jump && _isJumping && _isGrounded)
        {
            _isJumping = false;
        
            _coyoteTimeCounter = 0f;
        }
        
        // if (!_isJumping && _coyoteTimeCounter > 0f && _inputs.Jump) // _jumpBufferCounet > 0f à la place de _inputs.Jump
        // {
        //     _isJumping = true;
        //
        //     if (_planetMovement.PlanetMove)
        //     {
        //         _planetMovement.HandleJump();
        //     }
        //
        //     else
        //     {
        //         _rb.velocity = new Vector2(_rb.velocity.x, _initalJumpVelocity);
        //     }
        //
        //     _jumpBufferCounter = 0f;
        // }
        //
        // else if (!_inputs.Jump && _isJumping && _isGrounded)
        // {
        //     _isJumping = false;
        //
        //     _coyoteTimeCounter = 0f;
        // }
    }

    private IEnumerator JumpCooldown()
    {
        _canJump = false;
        yield return new WaitForSeconds(0.4f);
        _canJump = true;
    }

    public bool CheckGroundedState()
    {
        //_boxCastDistance = 0.05f; // 0.2f

        // Par rapport au pivot (dans le transform)
        RaycastHit2D raycastHit;
        
        
        raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
            _boxCollider.bounds.size, transform.rotation.z, _jumpController.BaseGravity,
            _boxCastDistance,
            _plateformLayerMask);
        
        // Debug part
        Color rayColor;
        
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

         Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x, 0), 
             _gravityVelocity.normalized * (_boxCollider.bounds.extents.y + _boxCastDistance), rayColor);
        
         Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x, 0), 
             _gravityVelocity.normalized * (_boxCollider.bounds.extents.y + _boxCastDistance), rayColor);
        
         Debug.DrawRay(_boxCollider.bounds.center - 
                       new Vector3(_boxCollider.bounds.extents.x, _boxCollider.bounds.extents.y + _boxCastDistance), 
             Vector2.right * (_boxCollider.bounds.extents.x * 2), rayColor);
        
        //Debug.Log(raycastHit.collider);
        
        return raycastHit.collider != null;
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Planet"))
        {
            _isCollidingAPlanet = true;
            _state = PlayerState.DEFAULT;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            _isCollidingAPlanet = false;
        }
    }


    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireCube(transform.position,
    //         _boxCollider.bounds.size);
    //     
    //     Gizmos.color = Color.yellow;
    // }
}