using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUI : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _livingTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LivingCo());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * (_speed * Time.deltaTime), Space.World);
    }

    private IEnumerator LivingCo()
    {
        yield return new WaitForSeconds(_livingTime);
        
        Destroy(gameObject);
    }
}
