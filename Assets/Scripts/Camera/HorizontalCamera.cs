using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class HorizontalCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _vCam;
    [SerializeField] private PlayerInputs _playerInputs;
    // [SerializeField] private Transform _playerTransform;
    // [SerializeField] private Transform _rightTrigger;
    // [SerializeField] private Transform _leftTrigger;

    private CinemachineFramingTransposer _cinemachineFramingTransposer;
    // Start is called before the first frame update
    void Start()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        _cinemachineFramingTransposer= _vCam.AddCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInputs.Move.x != 0f)
        {
            if (_playerInputs.Move.x > 0)
            {
                _cinemachineFramingTransposer.m_ScreenX = 0.25f;
            }
            else if (_playerInputs.Move.x < 0)
            {
                _cinemachineFramingTransposer.m_ScreenX = 0.75f;
            }
        }
        
    }
}
