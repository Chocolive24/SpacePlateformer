using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO mettre la gravité du monde à 0 quand on est sur les plaenetes 
//TODO refaire le jump pour qu'on puisse sauter dans une direction y inverse à la position du centre de la planète

public class PlanetMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _plateformLayerMask;
    [SerializeField] private JumpController jumpController;
    [SerializeField] private MovementController _playerMovement;

    [SerializeField] private PlanetGravity _planetGravity;
    
    private bool _planetMove = false;

    private Transform _planetTransform;
    
    public bool PlanetMove
    {
        get { return _planetMove; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleMove(float targetSpeed)
    {
        //Vector2 vectorToTarget = _planetTransform.position - transform.position;
        
        Vector2 vectorToTarget = (_planetTransform.position - transform.position) * 
            jumpController.BaseGravity;
        _playerMovement.RigidBody2D.AddForce(- vectorToTarget);
        
        // Vector2 rotateVectorToTarget = Quaternion.Euler(0, 0, 180) * vectorToTarget;
        // transform.rotation = Quaternion.LookRotation(Vector3.forward, rotateVectorToTarget);
        // transform.RotateAround(_planetTransform.position, Vector3.back, targetSpeed);
        //
        
        //
        transform.up = Vector3.MoveTowards(transform.up,  -vectorToTarget,
            jumpController.BaseGravity.y * Time.deltaTime);
    }

    public void HandleJump()
    {
        Vector2 vectorToTarget = _planetTransform.position - transform.position;
        
        //_playerMovement.SetVelocity(- vectorToTarget);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("GravityPointEffector"))
        {
            _planetMove = true;
            _planetTransform = col.gameObject.transform;

            //_gravityController.SetCurrentGravity(0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("GravityPointEffector"))
        {
            _planetMove = false;
            
            //_gravityController.SetCurrentGravity(_gravityController.LevelGravity);
        }
        
        
    }
}
