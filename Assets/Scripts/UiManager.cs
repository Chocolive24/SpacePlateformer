using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private CollectibleManager _collectibleManager;
    [SerializeField] private TextMeshProUGUI _coinCounterTxt;

    [SerializeField] private TextMeshProUGUI _scoreTxt;

    [SerializeField] private PlayerDataInt _score;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _scoreTxt.text = "Score : " + _score.Value.ToString();
        _coinCounterTxt.text = _collectibleManager.PickedUpCount.ToString();
    }
}
