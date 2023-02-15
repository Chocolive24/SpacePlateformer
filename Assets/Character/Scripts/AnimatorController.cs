using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] private MovementController _playerMovement;

    [SerializeField] private PlayerInputs _inputs;

    [SerializeField] private Animator _playerAnimator;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private JumpController _jumpController;
    
    private static readonly int Speed = Animator.StringToHash("speed");
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private bool _isJumpAnimated = false;
    private static readonly int Falling = Animator.StringToHash("Falling");

    public Animator PlayerAnimator
    {
        get => _playerAnimator;
        set => _playerAnimator = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_jumpController.HorizontalSense.y == 0)
        {
            HorizontalMoveAnim();
            
            if (_jumpController.GravitySense.y > 0)
            {
                if (!_playerMovement.IsOnAPlanetTrigger)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 180);
                }
            }

            else if (_jumpController.GravitySense.y < 0 && !_playerMovement.IsDead)
            {
                if (!_playerMovement.IsOnAPlanetTrigger)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
            
        }
        
        else if (_jumpController.HorizontalSense.y != 0)
        {
            VerticalMoveAnim();

            if (_jumpController.GravitySense.x > 0)
            {
                if (!_playerMovement.IsOnAPlanetTrigger)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
            }
            else if (_jumpController.GravitySense.x < 0)
            {
                if (!_playerMovement.IsOnAPlanetTrigger)
                {
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                }
            }
            
        }

        if (_inputs.Jump && !_isJumpAnimated)
        {
            _playerAnimator.SetBool(Jump, true);
            _isJumpAnimated = true;
        }

        if (_playerMovement.CheckGroundedState())
        {
            if (_isJumpAnimated)
            {
                _playerAnimator.SetBool(Jump, false);
                _isJumpAnimated = false;
            }
        }

        if (_playerMovement.IsFalling && !_playerMovement.CheckGroundedState())
        {
            _playerAnimator.SetBool(Falling, true);
            Debug.Log("ici");
        }
        else
        {
            _playerAnimator.SetBool(Falling, false);
        }
    }
    
    private void HorizontalMoveAnim()
    {
        if (_inputs.Move.x != 0f) // make the player look in the last direction he went
        {
            if (_inputs.Run)
            {
                _playerAnimator.SetBool(Run, true);
            }

            else
            {
                _playerAnimator.SetBool(Move, true);
            }
            
            //_spriteRenderer.flipX = _inputs.Move.x < 0;

            if (!_playerMovement.IsOnAPlanetTrigger)
            {
                if (_jumpController.GravitySense.y > 0)
                {
                    _spriteRenderer.flipX = _inputs.Move.x > 0;
                }
                else if (_jumpController.GravitySense.y < 0)
                {
                    _spriteRenderer.flipX = _inputs.Move.x < 0;
                }
            }
            
            else
            {
                _spriteRenderer.flipX = _inputs.Move.x < 0;
            }
            
            
            
            //_spriteRenderer.flipY = Vector2.Dot(transform.up, _jumpController.BaseGravity) > 0;
        }

        else
        {
            _playerAnimator.SetBool(Run, false);
            _playerAnimator.SetBool(Move, false);
        }
    }
    
    private void VerticalMoveAnim()
    {
        if (_inputs.Move.y != 0f) // make the player look in the last direction he went
        {
            if (_inputs.Run)
            {
                _playerAnimator.SetBool(Run, true);
            }

            else
            {
                _playerAnimator.SetBool(Move, true);
            }

            // if (_inputs.Move.x == 0f)
            // {

                if (!_playerMovement.IsOnAPlanetTrigger)
                {
                    if (_jumpController.GravitySense.x > 0)
                    {
                        _spriteRenderer.flipX = _inputs.Move.y < 0;
                    }
                    else if (_jumpController.GravitySense.x < 0)
                    {
                        _spriteRenderer.flipX = _inputs.Move.y > 0;
                    }
                }
                
                else
                {
                    _spriteRenderer.flipX = _inputs.Move.x > 0;
                }
                
            //}
            // else
            // {
            //     _spriteRenderer.flipX = _inputs.Move.x < 0;
            // }
            
            
        }
        
        else
        {
            _playerAnimator.SetBool(Run, false);
            _playerAnimator.SetBool(Move, false);
        }
    }

    public void RotatePlayer()
    {
        if (_jumpController.GravitySense.x > 0)
        {
            transform.Rotate(0, 0, 90);
        }
        else if (_jumpController.GravitySense.x < 0)
        {
            transform.Rotate(0, 0, -90);
        }
        else if (_jumpController.GravitySense.y < 0)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public void ResetPlayerRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
