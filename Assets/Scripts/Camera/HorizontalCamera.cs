using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class HorizontalCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _vCam;
    [SerializeField] private PlayerInputs _playerInputs;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _rightTrigger;
    [SerializeField] private GameObject _leftTrigger;

    private CinemachineFramingTransposer _cinemachineFramingTransposer;
    
    // Start is called before the first frame update
    void Start()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        _cinemachineFramingTransposer= _vCam.AddCinemachineComponent<CinemachineFramingTransposer>();
        
        _cinemachineFramingTransposer.m_ScreenX = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        // if (_playerTransform.position.x > _rightTrigger.transform.position.x)
        // {
        //     _vCam.m_Follow = _playerTransform;
        //     _rightTrigger.SetActive(false);
        //     _leftTrigger.SetActive(true);
        //     
        //     _cinemachineFramingTransposer.m_ScreenX = 0.25f;
        // }
        // else if (_playerTransform.position.x < _leftTrigger.transform.position.x)
        // {
        //     _vCam.m_Follow = _playerTransform;
        //     _rightTrigger.SetActive(true);
        //     _leftTrigger.SetActive(false);
        //     
        //     _cinemachineFramingTransposer.m_ScreenX = 0.75f;
        // }
        // else
        // {
        //     _vCam.m_Follow = null;
        //     
        //     
        // }
        //
        // Debug.Log(_leftTrigger.transform.position.x);
        
        // if (_playerInputs.Move.x != 0f)
        // {
        //     if (_playerInputs.Move.x > 0)
        //     {
        //         _cinemachineFramingTransposer.m_ScreenX = 0.25f;
        //     }
        //     else if (_playerInputs.Move.x < 0)
        //     {
        //         _cinemachineFramingTransposer.m_ScreenX = 0.75f;
        //     }
        // }
        
    }
}
