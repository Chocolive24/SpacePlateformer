using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStomp : MonoBehaviour
{
    [SerializeField] private MovementController _playerMovement;
    [SerializeField] private JumpController _jumpController;
    [SerializeField] private PlayerInputs _playerInputs;

    private bool _mustChangePosUp = true;
    private bool _mustChangePosDown = false;

    private Vector3 _originalPos;
    private Vector3 _upPos;
    private float _origianlYPos;
    
    // Start is called before the first frame update
    void Start()
    {
        _origianlYPos = transform.position.y;
        // _originalPos = transform.position;
        // _upPos = _originalPos;
        // _upPos.y += 1;
    }

    // Update is called once per frame
    void Update()
    {
        // if (!_playerMovement.IsDead)
        // {
        //     if (_jumpController.BaseGravity.y > 0 && _mustChangePosUp && !_playerMovement.IsOnAPlanetTrigger)
        //     {
        //         //transform.position = _upPos;
        //         var transformPosition = transform.position;
        //         transformPosition.y += 1;
        //         transform.position = transformPosition;
        //         _mustChangePosUp = false;
        //         _mustChangePosDown = true;
        //     }
        //     else if (_jumpController.BaseGravity.y < 0 && _mustChangePosDown && !_playerMovement.IsOnAPlanetTrigger)
        //     {
        //         //transform.position = _originalPos;
        //         var transformPosition = transform.position;
        //         transformPosition.y -= 1;
        //         transform.position = transformPosition;
        //         _mustChangePosUp = true;
        //         _mustChangePosDown = false;
        //     }
        // }
        // else
        // {
        //     transform.position = new Vector3(transform.position.x, _origianlYPos, transform.position.z);
        // }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.CompareTag("Alien"))
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();

            if (enemy)
            {
                StartCoroutine(enemy.DeathCo());
            }
            
            if (_playerInputs.Jump)
            {
                _playerMovement.GravityVelocity = _jumpController.InitialJumpVelocity;
            }
            else
            {
                _playerMovement.GravityVelocity = _jumpController.InitialJumpVelocity;
            }
        }
    }
}
