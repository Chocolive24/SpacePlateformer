using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStomp : MonoBehaviour
{
    [SerializeField] private MovementController _playerMovement;
    [SerializeField] private JumpController _jumpController;
    [SerializeField] private PlayerInputs _playerInputs;
    
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
