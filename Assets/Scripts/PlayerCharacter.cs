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
    private InputAction _equipGunAction;
    private InputAction _equipKnifeAction;
    private InputAction _equipEmptyAction;
    private float _sprintSpeed;
    private float _movementSpeed;
    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void Awake()
    {
        base.Awake();
        GameMaster.Player = gameObject;
        if (!_inputAsset) return;

        //Bind the actions to the input asset
        BindActions();

        //we bind a callback to it instead of continuously monitoring input
        _jumpAction.performed += HandleJumpInput;
        _equipGunAction.performed += context => HandleSwitchWeapon(AttackBehaviour.WeaponType.Gun);
        _equipKnifeAction.performed += context => HandleSwitchWeapon(AttackBehaviour.WeaponType.Knife);
        _equipEmptyAction.performed += context => HandleSwitchWeapon(AttackBehaviour.WeaponType.Empty);
        
        _sprintSpeed = _movementBehaviour.MovementSpeed * 2f;
        _movementSpeed = _movementBehaviour.MovementSpeed + GameMaster.Instance.PlayerUpgradeManager.MovementIncrease;
    }
    
    private void BindActions()
    {
        _jumpAction = _inputAsset.FindActionMap("Gameplay").FindAction("Jump");
        _shootAction = _inputAsset.FindActionMap("Gameplay").FindAction("Shoot");
        _xmovementAction = _inputAsset.FindActionMap("Gameplay").FindAction("XMovement");
        _zmovementAction = _inputAsset.FindActionMap("Gameplay").FindAction("ZMovement");
        _sprintAction = _inputAsset.FindActionMap("Gameplay").FindAction("Sprint");
        _equipGunAction = _inputAsset.FindActionMap("Gameplay").FindAction("EquipGun");
        _equipKnifeAction = _inputAsset.FindActionMap("Gameplay").FindAction("EquipKnife");
        _equipEmptyAction = _inputAsset.FindActionMap("Gameplay").FindAction("EquipEmpty");
    }

    protected void OnDestroy()
    {
        _jumpAction.performed -= HandleJumpInput;
        _equipGunAction.performed -= context => HandleSwitchWeapon(AttackBehaviour.WeaponType.Gun);
        _equipKnifeAction.performed -= context => HandleSwitchWeapon(AttackBehaviour.WeaponType.Knife);
        _equipEmptyAction.performed -= context => HandleSwitchWeapon(AttackBehaviour.WeaponType.Empty);
        //if health is 0 we need to trigger game over
        if (GetComponent<Health>().CurrentHealth <= 0) GameMaster.Instance.TriggerGameOver();
    }

    private void OnEnable()
    {
        _inputAsset?.Enable();
    }

    private void OnDisable()
    {
        _inputAsset?.Disable();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleAttackInput();
    }

    private void HandleMovementInput()
    {
        if (!_movementBehaviour || _xmovementAction == null || _zmovementAction == null) return;

        //movement
        float XmovementInput = _xmovementAction.ReadValue<float>();
        float ZmovementInput = _zmovementAction.ReadValue<float>();
        
        Vector3 movement = XmovementInput * transform.right;
        movement += ZmovementInput * transform.forward;
        _movementBehaviour.MovementSpeed = _sprintAction.IsPressed() ? _sprintSpeed : _movementSpeed;
        
        _movementBehaviour.DesiredMovementDirection = movement;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        _movementBehaviour.Jump();
    }
    private void HandleSwitchWeapon(AttackBehaviour.WeaponType weaponIndex)
    {
        _attackBehaviour?.SwitchWeapon(weaponIndex);
    }

    private void HandleAttackInput()
    {
        if (!_attackBehaviour || _shootAction == null) return;

        if (_shootAction.IsPressed()) _attackBehaviour.Attack();
    }
}