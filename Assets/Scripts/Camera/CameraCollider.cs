using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    // [SerializeField] private MovementController _playerMovement;
    //
    // [SerializeField] private Transform _rightScrollTrans;
    // [SerializeField] private Transform _leftScrollTrans;

    private CinemachineVirtualCamera _virtualCamera;
    
    private Rigidbody2D _rb;

    private bool _canMove = false;
    private Component _bodyAttributes;

    // Start is called before the first frame update
    void Start()
    {
        //_rb = GetComponent<Rigidbody2D>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
        _bodyAttributes = _virtualCamera.GetComponentInChildren(typeof(CinemachineFramingTransposer));
    }

    // Update is called once per frame
    void Update()
    {
        
        // if (_playerMovement.transform.position.x >= 0)
        // {
        //     _canMove = true;
        // }
        //
        // if (_canMove)
        // {
        //     HandleMove();
        // }
    }

    // private void HandleMove()
    // {
    //     if (_playerMovement.transform.position.x >= _rightScrollTrans.position.x ||
    //         _playerMovement.transform.position.x <= _leftScrollTrans.position.x)
    //     {
    //         _rb.velocity = new Vector2(_playerMovement.RigidBody2D.velocity.x, 0f);
    //     }
    //     else
    //     {
    //         _rb.velocity = Vector2.zero;
    //     }
    // }
    //
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.GetComponent<MovementController>() != null)
    //     {
    //        
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.GetComponent<MovementController>() != null)
    //     {
    //         
    //     }
    // }
}
