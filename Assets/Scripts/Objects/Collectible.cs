using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private float _animationTime = 0.5f;
    private bool _canBeAnimated = false;

    public event Action OnPickup;

    public bool CanBeAnimated { get { return _canBeAnimated; } }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.CompareTag("Player");
        
        if (player)
        {
            if (!_canBeAnimated)
            {
                // les méthodes abonnées à l'événement vont se lancer.
                OnPickup?.Invoke();
                
                StartCoroutine(nameof(DisableCo));
            }
        }
    }

    private IEnumerator DisableCo()
    {
        _canBeAnimated = true;
        
        yield return new WaitForSeconds(_animationTime);
        
        gameObject.SetActive(false);
    }
}
