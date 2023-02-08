using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerDatas/FloatData", fileName = "new data")]
public class PlayerDataFloat : ScriptableObject
{
    private float _value;
    public float Value { get => _value; set => _value = value; }
}
