using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _fireworkPrefab;

    [SerializeField] private float _timeBetweenSpawns;
    [SerializeField] private float _spawnLengthTime;

    private float _time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _timeBetweenSpawns)
        {
            _time = 0f;

            Vector3 rndSpawnPoint = new Vector3(Random.Range(transform.position.x - 10,
                transform.position.x -2), Random.Range(transform.position.y - 5,
                transform.position.y + 5), 0f);

            Instantiate(_fireworkPrefab, rndSpawnPoint, Quaternion.identity);
        }
    }
}
