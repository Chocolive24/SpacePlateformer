using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [NonSerialized] public Vector2 Move;

    [NonSerialized] public bool Run;

    [NonSerialized] public bool Jump;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    void OnRun(InputValue value)
    {
        Run = value.isPressed;
    }

    void OnJump(InputValue value)
    {
        Jump = value.isPressed;
    }
    
    private void MoveInput(Vector2 newMoveDirection)
    {
        Move = newMoveDirection;
    } 
    
    
}
