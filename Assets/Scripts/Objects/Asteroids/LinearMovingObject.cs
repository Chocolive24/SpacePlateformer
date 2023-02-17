using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovingObject : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _patternTime;
    [SerializeField] private float _pauseTime = 0f;

    private Rigidbody2D _rb;
    
    private float _time;

    private bool _isInPause = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        if (!_isInPause)
        {
            _time += Time.deltaTime;

            _rb.velocity = _direction * _speed;
            
            //transform.Translate(_direction * (_speed * Time.deltaTime));
        
            if (_time >= _patternTime)
            {
                _time = 0f;

                StartCoroutine(nameof(Pause));
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private IEnumerator Pause()
    {
        _isInPause = true;
        
        yield return new WaitForSeconds(_pauseTime);

        _isInPause = false;
        
        _direction = -_direction;
    }
}
