using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector2 _direction = Vector2.left;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _deathPart;
    [SerializeField] private Animator _animator;

    private CapsuleCollider2D _capsuleCollider;
    private Rigidbody2D _rb;

    [SerializeField] private float _walkTime;
    private float _time;
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private bool _dead = false;


    // Start is called before the first frame update
    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
    }

    private void FixedUpdate()
    {
        if (!_dead)
        {
            _rb.velocity = _direction * _speed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void HandleMove()
    {
        if (!_dead)
        {
            _time += Time.deltaTime;
            if (_time >= _walkTime)
            {
                _time = 0f;
                _direction = - _direction;
            }

            if (_direction.x < 0f)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
    }
    
    public IEnumerator DeathCo()
    {
        // play a sound
        _dead = true;
        _rb.velocity = Vector2.zero;
        _animator.SetBool(IsDead, true);
        _capsuleCollider.enabled = false;
        
        yield return new WaitForSeconds(0.5f);
        
        Destroy(gameObject);
    }
}
