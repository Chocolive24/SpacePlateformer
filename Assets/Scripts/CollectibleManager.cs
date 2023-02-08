using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private List<Collectible> _diamonds;

    private int _pickedUpCount;

    [SerializeField] private UnityEvent _allCoinsCollected;
    
    [SerializeField] private PlayerDataInt _score;
    
    // Getters and Setters -----------------------------------------------------------------
    public int PickedUpCount { get => _pickedUpCount; set => _pickedUpCount = value; }

    // Start is called before the first frame update
    void Start()
    {
        _diamonds = GetComponentsInChildren<Collectible>().ToList();
        
        // On s'abonne à la méthode.
        foreach (Collectible diamond in _diamonds)
        {
            diamond.OnPickup += HandlePickUp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        //On se desabonne à la méthode.
        foreach (Collectible diamond in _diamonds)
        {
            diamond.OnPickup -= HandlePickUp;
        }
    }

    void HandlePickUp()
    {
        _pickedUpCount++;
        SetScore();
        
        if (_pickedUpCount >= _diamonds.Count)
        {
            _allCoinsCollected.Invoke();
        }
    }

    void SetScore()
    {
        _score.Value++;
    }
    
}
