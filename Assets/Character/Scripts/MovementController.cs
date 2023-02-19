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
    [SerializeField] private JumpBuffer _jumpBuffer;
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

    private bool _isDead = false;
    
    private bool _isOnAPlanetTrigger = false;
    private bool _isCollidingAPlanet = false;

    private PlayerState _state = PlayerState.DEFAULT;
    private bool _canJump = true;

    [SerializeField] private float _jumpButtonGracePeriod = 0.2f;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool _isOnMovingPlatform;

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

    public bool IsFalling { get => _isFalling; set => _isFalling = value; }

    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

    public bool IsDead { get => _isDead; set => _isDead = value; }

    public Rigidbody2D RigidBody2D { get { return _rb; } }

    public bool IsOnAPlanetTrigger { get => _isOnAPlanetTrigger; set => _isOnAPlanetTrigger = value; }

    public bool IsCollidingAPlanet { get => _isCollidingAPlanet; set => _isCollidingAPlanet = value; }

    public bool IsOnMovingPlatform
    {
        get => _isOnMovingPlatform;
        set => _isOnMovingPlatform = value;
    }

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

        if (!_isDead)
        {
            HandleMoveHorizontal();
            HandleGravity();
            HandleJump();
        }
        else
        {
            _appliedGravity = Vector2.zero;
            _previousYVelocity = Vector2.zero;
            _gravityVelocity = Vector2.zero;
        }
        
        Debug.Log(_gravityVelocity + " " + _isJumping);
    }

    private void FixedUpdate()
    {
        // Update the physic of the game with the input of the update.

        if (!_isDead)
        {
            _rb.velocity = _gravityVelocity + _movementVelocity;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }

        // if (!_boxCollider.enabled)
        // {
        //     _rb.velocity = Vector2.zero;
        // }

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
                    _movementVelocity = transform.right * 
                                        (_jumpController.HorizontalSense.x * (_inputs.Move.x * targetSpeed));
                }
                // Vertical Movements for the Vector of movement
                else if (_jumpController.HorizontalSense.y != 0)
                {
                    _movementVelocity = transform.right * 
                                        (_jumpController.HorizontalSense.y * (_inputs.Move.y * targetSpeed));
                }
        }
        else
        {
            _movementVelocity = Vector2.zero;
        }
    }
    
    private void HandleGravity()
    {
        if (!_isOnMovingPlatform && !_inputs.Jump)
        {
            _isFalling = Vector2.Dot(_gravityVelocity, _jumpController.BaseGravity) > 0;
        }
        
        if (_isGrounded && !_isJumping && !_isOnMovingPlatform)
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

        if (State == PlayerState.DEFAULT && !_isOnMovingPlatform)
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
                    //_appliedGravity = _jumpController.BaseGravity;
                    _appliedGravity = _jumpController.JumpGravity;
                }

                Vector2 newYVelocity = _appliedGravity * Time.deltaTime;
                Vector2 verletVelocity = (newYVelocity + _previousYVelocity) * 0.5f;
                
                if (Mathf.Abs(verletVelocity.x) <= 20f && Mathf.Abs(verletVelocity.y) <= 20f)
                {
                    _previousYVelocity = newYVelocity;

                    _gravityVelocity += verletVelocity;
                }
                
                // else if (Mathf.Abs(verletVelocity.x) <= 20f && Mathf.Abs(verletVelocity.y) <= 20f
                //                                             && _isOnAPlanetTrigger)
                // {
                //     _gravityVelocity /= 2;
                //
                //     _gravityVelocity += verletVelocity /= 2;
                // }
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
                //_gravityVelocity += _jumpController.BaseGravity * Time.deltaTime;
                _gravityVelocity += _jumpController.JumpGravity * Time.deltaTime;
            }
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
        if (_inputs.Jump && _coyoteTimeCounter > 0f && !_isJumping || 
            _inputs.Jump && _jumpBuffer.CanJump && !_isJumping) 
        {
            _gravityVelocity = _jumpController.InitialJumpVelocity;
            _isJumping = true;
        }
        else if (!_inputs.Jump && _isJumping && _isGrounded || 
                 !_inputs.Jump && _isJumping && _jumpBuffer.CanJump) 
        {
            _isJumping = false;
        
            _coyoteTimeCounter = 0f;
        }
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
            _boxCollider.bounds.size, 0f, 
            _jumpController.BaseGravity,
            _boxCastDistance,
            _plateformLayerMask); // - new Vector3(0.1f, 0f, 0f)
        
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

         Debug.DrawRay(_boxCollider.bounds.center + new Vector3(_boxCollider.bounds.extents.x, 0) - new Vector3(0.1f, 0, 0), 
             _gravityVelocity.normalized * (_boxCollider.bounds.extents.y + _boxCastDistance) , rayColor);
        
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
        
        // if (col.gameObject.CompareTag("MovingPlatform"))
        // {
        //     if (Vector2.Dot(col.gameObject.GetComponent<Rigidbody2D>().velocity, _jumpController.BaseGravity) > 0)
        //     {
        //         _isOnMovingPlatform = true;
        //         
        //         // if (!_isJumping)
        //         // {
        //         //     _gravityVelocity = col.gameObject.GetComponent<Rigidbody2D>().velocity * 2;
        //         // }
        //     }
        // }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("MovingPlatform"))
        {
            if (Vector2.Dot(col.gameObject.GetComponent<Rigidbody2D>().velocity, _jumpController.BaseGravity) > 0)
            {
                _isOnMovingPlatform = true;
                
                _movementVelocity = col.gameObject.GetComponent<Rigidbody2D>().velocity;
                
                if (!_inputs.Jump)
                {
                     
                    _gravityVelocity = col.gameObject.GetComponent<Rigidbody2D>().velocity * 4;
                    Debug.Log(col.gameObject.GetComponent<Rigidbody2D>().velocity);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            _isCollidingAPlanet = false;
        }
        
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            _isOnMovingPlatform = false;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            _isOnMovingPlatform = false;
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
