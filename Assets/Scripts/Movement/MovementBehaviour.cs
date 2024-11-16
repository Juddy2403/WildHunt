using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] protected GameObject _shoulderObject = null;
    [SerializeField] protected float _movementSpeed = 1.0f;
    [SerializeField] protected float _jumpStrength = 5.0f;
    [SerializeField] private Camera _fpsCamera;

    protected Rigidbody _rigidBody;
    protected Vector3 _desiredMovementDirection = Vector3.zero;
    protected float _desiredXRotation = 0.0f;
    protected Transform _target;

    protected bool _grounded = false;
    public bool CanMove { get; set; } = true;
    protected const float GROUND_CHECK_DISTANCE = 0.2f;
    protected const string GROUND_LAYER = "Ground";

    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }

    public virtual float MovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if(CanMove) HandleLookAt();
    }

    protected virtual void FixedUpdate()
    {
        if (CanMove) HandleMovement();
        //check if there is ground beneath our feet
        _grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GROUND_CHECK_DISTANCE,
            LayerMask.GetMask(GROUND_LAYER));
    }

    protected virtual void HandleMovement()
    {
        if (!_rigidBody) return;

        Vector3 movement = _desiredMovementDirection.normalized;
        movement *= _movementSpeed;
        //maintain vertical velocity as it was otherwise gravity would be stripped out
        movement.y = _rigidBody.velocity.y;
        _rigidBody.velocity = movement;
    }

    public void PushBackwards(Vector3 bulletForward)
    {
        Vector3 force = bulletForward * 10f + Vector3.up * 10f; // Adjust the multipliers as needed
        _rigidBody.velocity = force;
        CanMove = false;
        Invoke(nameof(EnableMovement), 0.7f);
    }

    private void EnableMovement()
    {
        CanMove = true;
    }

    private void HandleLookAt()
    {
        if (!_shoulderObject) return;

        const float lookSpeed = 120f;
        const float lookLimit = 80f;
        float rotationX = -Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
        _desiredXRotation += rotationX;
        _desiredXRotation = Mathf.Clamp(_desiredXRotation, -lookLimit, lookLimit);

        _fpsCamera.transform.localRotation = Quaternion.Euler(_desiredXRotation, 0, 0);
        _shoulderObject.transform.localRotation = _fpsCamera.transform.localRotation * Quaternion.Euler(0, 0.75f, 0);

        // Move camera left and right; player object follows camera rotation
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime, 0);
    }

    public void Jump()
    {
        if (_grounded) _rigidBody.AddForce(Vector3.up * _jumpStrength, ForceMode.Impulse);
    }
}