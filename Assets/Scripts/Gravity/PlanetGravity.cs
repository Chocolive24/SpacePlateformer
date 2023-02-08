using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    [SerializeField] private float _gravityScale = 2f; 
    [SerializeField] private float _planetRadius;
    
    private Vector2 _planetGravitySense;

    private GameObject _player;
    private MovementController _movementController;
    private JumpController _jumpController;
    private PlayerInputs _playerInputs;

    private bool _hasCollideWithPlayer = false;

    private Vector2 _correctionVector;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _movementController = col.gameObject.GetComponent<MovementController>();
            
            _movementController.State = PlayerState.CHANGING_GRAVITY;
            
            // Player's state is set to default in the OnCollisionEnter with the collider part of the planet in the
            // MovementController script.
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _player = col.gameObject;
            _movementController = col.gameObject.GetComponent<MovementController>();
            _jumpController = col.gameObject.GetComponent<JumpController>();
            _playerInputs = col.gameObject.GetComponent<PlayerInputs>();
            
            _planetGravitySense = (transform.position - _player.transform.position).normalized;
            
            _movementController.IsOnAPlanetTrigger = true;
            
            float angle = 90 + Mathf.Atan2(_planetGravitySense.y, _planetGravitySense.x) * Mathf.Rad2Deg;
            _player.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (_movementController.State == PlayerState.CHANGING_GRAVITY)
            {
                _correctionVector = new Vector2(1, 1.5f);
            }
            else
            {
                _correctionVector = new Vector2(1, 1);
            }
            
            _jumpController.SetGravitySenseToPlanet(- (_player.transform.up * _correctionVector));

            // if (!_movementController.IsGrounded && !_hasCollideWithPlayer)
            // {
            //     _hasCollideWithPlayer = _movementController.IsGrounded;
            //     _movementController.MovementVelocity = Vector2.zero;
            //     _jumpController.InitialJumpVelocity = Vector2.zero;
            //
            //     _playerInputs.enabled = false;
            // }
            // else
            // {
            //     _playerInputs.enabled = true;
            // }

            // _player.transform.up = Vector3.MoveTowards(_player.transform.up, -(_planetGravitySense),
            //     100);


            //_animatorController.RotatePlayer();



            // //_jumpController = col.GetComponent<JumpController>();
            //

            //
            // _player.GetComponent<JumpController>().SetGravitySense(DirectionEnum.RIGHT, 
            //     DirectionEnum.RIGHT);
            //
            // //_jumpController.SetGravitySenseToPlanet(_planetGravitySense);
        }
        
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            MovementController movementController = other.gameObject.GetComponent<MovementController>();
            JumpController jumpController = other.gameObject.GetComponent<JumpController>();

            movementController.IsOnAPlanetTrigger = false;
            // _movementController.RigidBody2D.drag = 0f;
            jumpController.ResetGravitySense();
            player.transform.rotation = Quaternion.identity;
        }
    }
}
