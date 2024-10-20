using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField] private InputActionAsset _inputAsset;

    private InputAction _jumpAction;
    private InputAction _shootAction;
    private InputAction _xmovementAction;
    private InputAction _zmovementAction;
    private InputAction _sprintAction;
    private InputAction _switchWeaponAction;
    private float _sprintSpeed;
    private float _movementSpeed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void Awake()
    {
        base.Awake();
        if (!_inputAsset) return;

        //Bind the actions to the input asset
        _jumpAction = _inputAsset.FindActionMap("Gameplay").FindAction("Jump");
        _shootAction = _inputAsset.FindActionMap("Gameplay").FindAction("Shoot");
        _xmovementAction = _inputAsset.FindActionMap("Gameplay").FindAction("XMovement");
        _zmovementAction = _inputAsset.FindActionMap("Gameplay").FindAction("ZMovement");
        _sprintAction = _inputAsset.FindActionMap("Gameplay").FindAction("Sprint");
        _switchWeaponAction = _inputAsset.FindActionMap("Gameplay").FindAction("SwitchWeapon");

        //we bind a callback to it instead of continiously monitoring input
        _jumpAction.performed += HandleJumpInput;
        _switchWeaponAction.performed += HandleSwitchWeapon;
        
        _sprintSpeed = _movementBehaviour.MovementSpeed * 2f;
        _movementSpeed = _movementBehaviour.MovementSpeed;
    }

    protected void OnDestroy()
    {
        _jumpAction.performed -= HandleJumpInput;
        _switchWeaponAction.performed -= HandleSwitchWeapon;
    }


    private void OnEnable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Enable();
    }

    private void OnDisable()
    {
        if (_inputAsset == null) return;

        _inputAsset.Disable();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleAttackInput();
        HandleAimingInput();
    }

    void HandleMovementInput()
    {
        if (!_movementBehaviour || _xmovementAction == null || _zmovementAction == null) return;

        //movement
        float XmovementInput = _xmovementAction.ReadValue<float>();
        float ZmovementInput = _zmovementAction.ReadValue<float>();
        
        Vector3 movement = XmovementInput * transform.right;
        movement += ZmovementInput * transform.forward;
        if(_sprintAction.IsPressed()) _movementBehaviour.MovementSpeed = _sprintSpeed;
        else _movementBehaviour.MovementSpeed = _movementSpeed;
        
        _movementBehaviour.DesiredMovementDirection = movement;
    }

    private void HandleAimingInput()
    {
        float rotationX = -Input.GetAxis("Mouse Y");
        _movementBehaviour.DesiredXRotation = rotationX;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        _movementBehaviour.Jump();
    }
    private void HandleSwitchWeapon(InputAction.CallbackContext context)
    {
        _attackBehaviour?.SwitchWeapon();
    }

    private void HandleAttackInput()
    {
        if (!_attackBehaviour || _shootAction == null) return;

        if (_shootAction.IsPressed()) _attackBehaviour.Attack();
    }
}