using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    [SerializeField] private DirectionEnum _horizontalDirection;
    [SerializeField] private DirectionEnum _gravityDirection;
    
    private GameObject _player;
    private JumpController _jumpController;
    private AnimatorController _animatorController;
    private MovementController _movementController;
    private PlayerInputs _playerInputs;

    private float _degreesToRotate;
    private static readonly int RotateUp = Animator.StringToHash("RotateUp");

    // Start is called before the first frame update
    void Start()
    {
        if (_gravityDirection == DirectionEnum.UP)
        {
            _degreesToRotate = 180f;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _player = col.gameObject;
            
            _movementController = col.gameObject.GetComponent<MovementController>();
            _jumpController = col.gameObject.GetComponent<JumpController>();
            _animatorController = col.gameObject.GetComponent<AnimatorController>();
            _playerInputs = col.gameObject.GetComponent<PlayerInputs>();
            
            StartCoroutine(GravityChangeCo(true));

            // _jumpController.SetGravitySense(_horizontalDirection, _gravityDirection);
            // _animatorController.RotatePlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(GravityChangeCo(false));
            
            // _jumpController.ResetGravitySense();
            // _animatorController.ResetPlayerRotation();
            // _movementController.RigidBody2D.velocity = Vector2.zero;
        }
    }

    private IEnumerator GravityChangeCo(bool enter)
    {
        //_movementController.State = PlayerState.CHANGING_GRAVITY;
        _jumpController.SetGravitySenseToPlanet(Vector2.zero);
        _movementController.MovementVelocity -= _movementController.MovementVelocity.normalized * 4;
        _movementController.GravityVelocity -= _movementController.GravityVelocity.normalized * 8;
        //_movementController.GravityVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.2f);
        
        //_movementController.State = PlayerState.DEFAULT;
        
        if (enter)
        {
            _jumpController.SetGravitySense(_horizontalDirection, _gravityDirection);
            _animatorController.RotatePlayer();
        }
        else
        {
            _jumpController.ResetGravitySense();
            _animatorController.ResetPlayerRotation();
        }
    }
}
