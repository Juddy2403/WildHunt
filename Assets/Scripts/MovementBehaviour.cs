using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] protected GameObject _shoulderObject = null;
    [SerializeField] protected float _movementSpeed = 1.0f;
    [SerializeField] protected float _jumpStrength = 10.0f;
    [SerializeField] private Camera _fpsCamera;

    protected Rigidbody _rigidBody;
    protected Vector3 _desiredMovementDirection = Vector3.zero;
    protected float _desiredXRotation = 0.0f;
    protected GameObject _target;

    protected bool _grounded = false;
    protected bool _canMove = true;
    protected const float GROUND_CHECK_DISTANCE = 0.2f;
    protected const string GROUND_LAYER = "Ground";

    public Vector3 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }
    public float DesiredXRotation
    {
        get { return _desiredXRotation; }
        set { _desiredXRotation = value; }
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
        HandleLookAt();
    }

    protected virtual void FixedUpdate()
    {
        if(_canMove) HandleMovement();
        //check if there is ground beneath our feet
        _grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, GROUND_CHECK_DISTANCE, LayerMask.GetMask(GROUND_LAYER));
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
        _canMove = false;
        Invoke(nameof(EnableMovement),1f);
    }

    private void EnableMovement()
    {
        _canMove = true;
    }

    protected virtual void HandleLookAt()
    {
        if (!_shoulderObject) return;
        
        float lookSpeed = 2f;
        float lookLimit = 45f;
        _desiredXRotation *= lookSpeed;
        
        if(_fpsCamera.transform.rotation.eulerAngles.x + _desiredXRotation < lookLimit || 
           _fpsCamera.transform.rotation.eulerAngles.x + _desiredXRotation > 360-lookLimit)
            _fpsCamera.transform.Rotate(Vector3.right, _desiredXRotation);
        
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        _shoulderObject.transform.rotation = _fpsCamera.transform.rotation * Quaternion.Euler(0, 0.75f, 0);
    }

    public void Jump()
    {
        if (_grounded) _rigidBody.AddForce(Vector3.up * _jumpStrength, ForceMode.Impulse);
    }
}




