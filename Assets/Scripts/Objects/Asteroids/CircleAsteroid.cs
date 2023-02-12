using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAsteroid : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private GameObject _planet;
    private Vector3 _planetPos;
    
    // Start is called before the first frame update
    void Start()
    {
        _planetPos = _planet.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        transform.RotateAround(_planetPos, new Vector3(0, 0, 1), _speed * Time.deltaTime);
    }

   
}
