using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDatas/IntData", fileName = "new data")]
public class PlayerDataInt : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private int _initialValue;
    private int _value;
    public int Value { get => _value; set => _value = value; }
    
    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        _value = _initialValue;
    }
    
}
