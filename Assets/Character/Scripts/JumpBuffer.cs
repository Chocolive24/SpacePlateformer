using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBuffer : MonoBehaviour
{
    [SerializeField] private LayerMask _plateformLayerMask;

    private bool _canJump;

    public bool CanJump => _canJump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CanJump);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Plateform"))
        {
            _canJump = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Plateform"))
        {
            _canJump = false;
        }
    }
}
