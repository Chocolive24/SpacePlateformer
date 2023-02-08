using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Unity.VisualScripting;
using UnityEngine;

public enum DirectionEnum
{
    RIGHT,
    LEFT,
    UP,
    DOWN,
    ZERO
}

public class JumpController : MonoBehaviour
{
    // Gravity variables ------------------------------------------------------------------------
    [SerializeField] private float _baseGravityFactor = 9.81f;

    private Vector2 _gravitySense = Vector2.down;
    private Vector2 _horizontalSense = Vector2.right;
    
    // Jump variables -----------------------------------------------------------------
    private Vector2 _initialJumpVelocity;
    [SerializeField] private float _maxJumpHeight;
    [SerializeField] private float _maxJumpTime;
    [SerializeField] private float _fallMultiplier = 2f;
    

    [SerializeField] private float _jumpFactor = -2f;
    [SerializeField] private float _fallFactor = 3f;
    
    private float _timeToApex;

    // Getters and Setters ----------------------------------------------------------------------
    public Vector2 BaseGravity { get { return _baseGravityFactor * _gravitySense; } }
    //public Vector2 InitialJumpVelocity { get { return _jumpFactor / _baseGravityFactor * -(_gravitySense.normalized); } }
    public Vector2 InitialJumpVelocity
    {
        get => ((2 * _maxJumpHeight) / _timeToApex) * -(_gravitySense.normalized);
        set => _initialJumpVelocity = value;
    }
    //public Vector2 JumpGravity { get {return _fallFactor  * _baseGravityFactor * _gravitySense;} }
    public Vector2 JumpGravity { get {return ((2 * _maxJumpHeight) / Mathf.Pow(_timeToApex, 2)) * _gravitySense;} }
    //public Vector2 JumpGravity { get {return ((2 * _maxJumpHeight) / Mathf.Pow(_timeToApex, 2)) * transform.up;} }
    public float FallMultiplier => _fallMultiplier;
    
    public Vector2 HorizontalSense { get { return _horizontalSense; } }

    public Vector2 GravitySense => _gravitySense;

    public void SetGravitySense(DirectionEnum horizontaleSense, DirectionEnum gravitySense)
    {
        // _gravitySense = new Vector2(0,sense.y);
        // _horizontalSense = new Vector2(sense.x, 0);

        _horizontalSense = DirectionToVector(horizontaleSense);
        _gravitySense = DirectionToVector(gravitySense);
    }
    
    public void SetGravitySenseToPlanet(Vector2 gravitySense)
    {
        _gravitySense = gravitySense;
    }
    
    // public void GravitySense(Vector2 horizontaleSense, Vector2 gravitySens)
    // {
    //     // _gravitySense = new Vector2(0,sense.y);
    //     // _horizontalSense = new Vector2(sense.x, 0);
    //
    //     _horizontalSense = horizontaleSense;
    //     _gravitySense = gravitySens;
    // }

    // ------------------------------------------------------------------------------------------
    
    // Start is called before the first frame update
    void Start()
    {
        _timeToApex = _maxJumpTime / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 DirectionToVector(DirectionEnum directionEnum)
    {
        switch(directionEnum)
        {
            case DirectionEnum.RIGHT:
                return Vector2.right;
            case DirectionEnum.LEFT:
                return Vector2.left;
            case DirectionEnum.UP:
                return Vector2.up;
            case DirectionEnum.DOWN:
                return Vector2.down;
            case DirectionEnum.ZERO:
                return Vector2.zero;
        }

        return default;
    }
    
    private void SetJumpVariables()
    {

        // float timeToApex = _maxJumpTime / (2f * Mathf.Abs(_jumpSenseFactor.y));
        //
        // float height = _maxJumpHeight / Mathf.Abs(_jumpSenseFactor.y);
        //
        // Debug.Log("Height :" + height);
        //
        // _jumpGravity.y = ((-2f * height) / (timeToApex * timeToApex));
        // _initalJumpVelocity.y = (2f * height) / timeToApex;

    }
    
    public void ResetGravitySense()
    {
        _gravitySense = new Vector2(0, -1);
        _horizontalSense = new Vector2(1, 0);
    }
}
