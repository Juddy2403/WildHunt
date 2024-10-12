using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementBehaviour : MonoBehaviour
{
    [SerializeField]
    protected GameObject _shoulderObject = null;

    [SerializeField]
    protected float _movementSpeed = 1.0f;

    [SerializeField]
    protected float _jumpStrength = 10.0f;

    protected Rigidbody _rigidBody;

    protected Vector3 _desiredMovementDirection = Vector3.zero;
    protected Vector3 _desiredLookatPoint = Vector3.zero;
    protected GameObject _target;

    protected bool _grounded = false;

    protected const float GROUND_CHECK_DISTANCE = 0.2f;
    protected const string GROUND_LAYER = "Ground";

    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }
    public Vector3 DesiredLookatPoint
    {
        get { return _desiredLookatPoint; }
        set { _desiredLookatPoint = value; }
    }
    public GameObject Target
    {
        get { return _target; }
        set { _target = value; }
    }


    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        HandleLookat();
    }


    protected virtual void FixedUpdate()
    {
        HandleMovement();

        //check if there is ground beneath our feet
        _grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GROUND_CHECK_DISTANCE, LayerMask.GetMask(GROUND_LAYER));
    }

    protected virtual void HandleMovement()
    {
        if (_rigidBody == null) return;

        Vector3 movement = _desiredMovementDirection.normalized;
        movement *= _movementSpeed;

        //maintain vertical velocity as it was otherwise gravity would be stripped out
        movement.y = _rigidBody.velocity.y;
        _rigidBody.velocity = movement;
    }

    protected virtual void HandleLookat()
    {
        if (_shoulderObject == null) return;

        _shoulderObject.transform.LookAt(_desiredLookatPoint);
    }

    public void Jump()
    {
        if (_grounded)
            _rigidBody.AddForce(Vector3.up * _jumpStrength, ForceMode.Impulse);
    }
}




