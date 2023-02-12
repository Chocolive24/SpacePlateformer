using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearAsteroid : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _patternTime;
    private float _time;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        _time += Time.deltaTime;

        transform.Translate(_direction * (_speed * Time.deltaTime));
        
        if (_time >= _patternTime)
        {
            _time = 0f;

            _direction = -_direction;

        }
    }
}
