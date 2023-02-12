using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _hitStomp;
    
    private Animator _animator;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private BoxCollider2D _boxCollider2D;
    private MovementController _movementController;
    private JumpController _jumpController;
    

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _movementController = GetComponent<MovementController>();
        _jumpController = GetComponent<JumpController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -6f)
        {
            transform.position = Vector3.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Dangerous") || col.gameObject.CompareTag("Alien"))
        {
            StartCoroutine(nameof(DeathCo));
        }
    }

    private IEnumerator DeathCo()
    {
        _movementController.IsDead = true;
        _movementController.IsFalling = false;
        _jumpController.SetGravitySenseToPlanet(Vector2.zero);
        _animator.SetBool(IsDead, true);
        _boxCollider2D.enabled = false;
        _hitStomp.enabled = false;
        
        yield return new WaitForSeconds(1f);
        
        _movementController.IsDead = false;
        _animator.SetBool(IsDead, false);
        _boxCollider2D.enabled = true;
        transform.position = Vector3.zero;
        _jumpController.ResetGravitySense();
        _hitStomp.enabled = true;
    }
}
