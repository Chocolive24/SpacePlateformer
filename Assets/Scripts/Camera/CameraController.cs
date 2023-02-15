using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _horizontalCam;
    [SerializeField] private CinemachineVirtualCamera _verticalCam;
    [SerializeField] private CinemachineVirtualCamera _endLvlCam;
    [SerializeField] private CinemachineVirtualCamera _planetCam;

    [SerializeField] private MovementController _playerMovement;
    [SerializeField] private JumpController _jumpController;

    [SerializeField] private LevelEnd _endLvl;
    
    // Start is called before the first frame update
    void Start()
    {
        _horizontalCam.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMovement.IsOnAPlanetTrigger)
        {
            _planetCam.enabled = true;
            _endLvlCam.enabled = false;
            _horizontalCam.enabled = false;
            _verticalCam.enabled = false;
        }
        else if (_endLvl.LevelFinished)
        {
            _planetCam.enabled = false;
            _endLvlCam.enabled = true;
            _horizontalCam.enabled = false;
            _verticalCam.enabled = false;
        }
        else
        {
            if (_jumpController.BaseGravity.x == 0f)
            {
                _planetCam.enabled = false;
                _endLvlCam.enabled = false;
                _horizontalCam.enabled = true;
                _verticalCam.enabled = false;
            }
            else if (_jumpController.BaseGravity.y == 0f)
            {
                _planetCam.enabled = false;
                _endLvlCam.enabled = false;
                _horizontalCam.enabled = false;
                _verticalCam.enabled = true;
            }
        }
        
    }
}
