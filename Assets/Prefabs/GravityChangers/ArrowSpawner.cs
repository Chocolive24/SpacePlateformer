using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _arrowPrefab;

    private float _time;
    [SerializeField] private float _spawnTime = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(_arrowPrefab, transform.position, _arrowPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _spawnTime)
        {
            _time = 0f;

            Instantiate(_arrowPrefab, transform.position, _arrowPrefab.transform.rotation);
        }

    }
}
