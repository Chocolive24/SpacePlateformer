using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _hitStomp;
    [SerializeField] private PolygonCollider2D _levelConfiner;
    
    private Animator _animator;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private BoxCollider2D _boxCollider2D;
    private MovementController _movementController;
    private JumpController _jumpController;
    private PlayerInputs _playerInputs;

    private Vector3 _respawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        _respawnPoint = transform.position;

        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _movementController = GetComponent<MovementController>();
        _jumpController = GetComponent<JumpController>();
        _playerInputs = GetComponent<PlayerInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (transform.position.y <= -6f)
        // {
        //     StartCoroutine(nameof(FallCo));
        // }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("CheckPoint"))
        {
            _respawnPoint = col.gameObject.transform.position;
            col.gameObject.GetComponent<Animator>().SetBool("Collected", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("LevelConfiner") && !_movementController.IsDead)
        {
            StartCoroutine(nameof(FallCo));
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Dangerous") || col.gameObject.CompareTag("Alien"))
        {
            StartCoroutine(nameof(DeathCo));
        }
    }

    private IEnumerator FallCo()
    {
        yield return new WaitForSeconds(0f);
    
        transform.position = _respawnPoint;
    }
    
    private IEnumerator DeathCo()
    {
        _movementController.IsDead = true;
        _movementController.IsFalling = false;
        
        //_jumpController.SetGravitySenseToPlanet(Vector2.zero);
        _movementController.RigidBody2D.velocity = Vector2.zero;
        
        _playerInputs.enabled = false;
        _boxCollider2D.enabled = false;
        _hitStomp.enabled = false;
        
        
        _animator.SetBool(IsDead, true);
        
        yield return new WaitForSeconds(1f);
        
        _animator.SetBool(IsDead, false);
        _boxCollider2D.enabled = true;
        
        transform.position = _respawnPoint;
        
        _jumpController.ResetGravitySense();
        _hitStomp.enabled = true;
        
        yield return new WaitForSeconds(0.3f);
        
        _movementController.IsDead = false;
        _playerInputs.enabled = true;
    }
}
