using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerWorkShop : MonoBehaviour
{
    // Rigid Body variables -----------------------------------------------------------
    private Rigidbody2D _rb;
    
    private Vector2 _velocity;
    
    // --------------------------------------------------------------------------------
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 5f;
    
    [SerializeField] private PlayerInputs _inputs;
    
    [SerializeField] private float _baseGravity = -9.81f;
    
    [SerializeField] private float _boxCastDistance = 0.05f;

    private bool _isGrounded;
    private bool _isWalled;
    private BoxCollider2D _boxCollider;
    [SerializeField] private LayerMask _plateformLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Methods to get inputs and do actions.
        // It's on the update in order to get as fast as possible the player's input and to response
        // to them as fast as possible.
        //HandleMoveHorizontal();

        _isGrounded = IsGrounded();
        
        HandleGravity();
    }

    private void HandleGravity()
    {
        if (!_isGrounded)
        {
            _velocity.y += _baseGravity * Time.deltaTime;
        }
        else
        {
            _velocity.y = 0f;
        }
        
        
        Debug.Log(_velocity.y);
        Debug.Log(_isGrounded);
    }

    private bool IsGrounded()
    {
        //_boxCastDistance = 0.05f; // 0.2f

        // Par rapport au pivot (dans le transform)
        RaycastHit2D raycastHit;
        
            raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
                _boxCollider.bounds.size, transform.rotation.z, Vector2.down, _boxCastDistance,
                _plateformLayerMask);

            //Physics2D.Raycast(transform.position, Vector2.down, _boxCastDistance, _plateformLayerMask);
            
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
            Vector2.down * (_boxCollider.bounds.extents.y + _boxCastDistance), rayColor);
        
        Debug.DrawRay(_boxCollider.bounds.center - new Vector3(_boxCollider.bounds.extents.x, 0), 
            Vector2.down * (_boxCollider.bounds.extents.y + _boxCastDistance), rayColor);
        
        Debug.DrawRay(_boxCollider.bounds.center - 
                      new Vector3(_boxCollider.bounds.extents.x, _boxCollider.bounds.extents.y + _boxCastDistance), 
            Vector2.right * (_boxCollider.bounds.extents.x * 2), rayColor);
        
        //Debug.Log(raycastHit.collider);
        
        return raycastHit.collider != null;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _velocity;
    }

    // private void HandleMoveHorizontal()
    // {
    //     float targetSpeed = _inputs.Run ? _runSpeed : _walkSpeed;
    //     
    //     // Don't need to apply Time.deltaTime because the velocity of the rigid body already do it.
    //     _velocity.x = _inputs.Move * targetSpeed;
    // }
    
    
    
    
    // private bool IsWalled()
    // {
    //     RaycastHit2D raycastHit;
    //     
    //     if (!_gravityController.HasGravityChanged)
    //     {
    //         raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
    //             _boxCollider.bounds.size, transform.rotation.z, Vector2.right, _boxCastDistance,
    //             _plateformLayerMask);
    //
    //         //Physics2D.Raycast(transform.position, Vector2.down, _boxCastDistance, _plateformLayerMask);
    //     }
    //
    //     else
    //     {
    //         raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
    //             _boxCollider.bounds.size, transform.rotation.z, Vector2.up, _boxCastDistance,
    //             _plateformLayerMask);
    //     }
    //     
    //     if (!_gravityController.HasGravityChanged)
    //     {
    //         raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
    //             _boxCollider.bounds.size, transform.rotation.z, Vector2.left, _boxCastDistance,
    //             _plateformLayerMask);
    //
    //         //Physics2D.Raycast(transform.position, Vector2.down, _boxCastDistance, _plateformLayerMask);
    //     }
    //
    //     else
    //     {
    //         raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center,
    //             _boxCollider.bounds.size, transform.rotation.z, Vector2.up, _boxCastDistance,
    //             _plateformLayerMask);
    //     }
    //
    //     return raycastHit.collider != null;
    // }
    
    // private void HandleYVelocity()
    // {
    //     float previousYVelocity = _rb.velocity.y;
    //
    //     var rbVelocity = _rb.velocity; 
    //     
    //     if (_isFalling || !_inputs.Jump)
    //     {
    //         rbVelocity.y = _rb.velocity.y + (_gravityController.CurrentGravity * _fallMultiplier * Time.deltaTime);
    //     }
    //
    //     else
    //     {
    //         rbVelocity.y = _rb.velocity.y + (_gravityController.CurrentGravity * Time.deltaTime);
    //     }
    //     
    //     _appliedYVelocity = Mathf.Max((previousYVelocity + rbVelocity.y) * 0.5f, -20f);
    //
    //     rbVelocity.y = _appliedYVelocity;              
    //                                        
    //     _rb.velocity = rbVelocity;
    // }
}
