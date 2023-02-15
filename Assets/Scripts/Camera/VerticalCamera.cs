using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VerticalCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _vCam;
    [SerializeField] private PlayerInputs _playerInputs;
    [SerializeField] private JumpController _jumpController;
    
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
        if (_playerInputs.Move.y != 0f)
        {
            
                if (_playerInputs.Move.y > 0)
                {
                    _cinemachineFramingTransposer.m_ScreenY = 0.8f;
                }
                else if (_playerInputs.Move.y < 0)
                {
                    _cinemachineFramingTransposer.m_ScreenY = 0.2f;
                }
            

            // else
            // {
            //     if (_jumpController.BaseGravity.x > 0f)
            //     {
            //         if (_playerInputs.Move.x > 0)
            //         {
            //             _cinemachineFramingTransposer.m_ScreenY = 0.8f;
            //         }
            //         else if (_playerInputs.Move.x < 0)
            //         {
            //             _cinemachineFramingTransposer.m_ScreenY = 0.2f;
            //         }
            //     }
            //     else if (_jumpController.BaseGravity.x < 0f)
            //     {
            //         if (_playerInputs.Move.x > 0)
            //         {
            //             _cinemachineFramingTransposer.m_ScreenY = 0.2f;
            //         }
            //         else if (_playerInputs.Move.x < 0)
            //         {
            //             _cinemachineFramingTransposer.m_ScreenY = 0.8f;
            //         }
            //     }
            // }
            
        }
    }
}
