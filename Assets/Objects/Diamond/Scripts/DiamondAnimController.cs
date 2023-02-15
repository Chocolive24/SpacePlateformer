using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondAnimController : MonoBehaviour
{
    private Animator _animator;
    private Collectible _diamond;

    private static readonly int IsCollected = Animator.StringToHash("IsCollected");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _diamond = GetComponent<Collectible>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_diamond.CanBeAnimated)
        {
            //_animator.SetBool(IsCollected, true);
            //transform.position += new Vector3(0, 0.01f, 0);
            transform.position += transform.up / 100;
            transform.Rotate(0f, 50f, 0f);
        }
        else
        {
            _animator.SetBool(IsCollected, false);
        }
    }
}
