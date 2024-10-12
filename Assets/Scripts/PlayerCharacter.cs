using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BasicCharacter
{
    [SerializeField]
    private InputActionAsset _inputAsset;

    [SerializeField]
    private InputActionReference _movementAction;

    private InputAction _jumpAction;
    private InputAction _shootAction;

    protected override void Awake()
    {
        base.Awake();

        if (_inputAsset == null) return;

        //example of searching for the bindings in code, alternatively, they can be hooked in the editor using a InputAcctionReference as shown by _movementAction
        _jumpAction = _inputAsset.FindActionMap("Gameplay").FindAction("Jump");
        _shootAction = _inputAsset.FindActionMap("Gameplay").FindAction("Shoot");

        //we bind a callback to it instead of continiously monitoring input
        _jumpAction.performed += HandleJumpInput;
    }
    protected void OnDestroy()
    {
        _jumpAction.performed -= HandleJumpInput;
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
        if (_movementBehaviour == null ||
            _movementAction == null)
            return;

        //movement
        float movementInput = _movementAction.action.ReadValue<float>();

        Vector3 movement = movementInput * Vector3.right;

        _movementBehaviour.DesiredMovementDirection = movement;
    }

    private void HandleAimingInput()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Mathf.Abs(transform.position.z - Camera.main.transform.position.z); // the mouse position represents a line, the z coordinate represents the distance from the camera we pick a point on this line
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0; //or game takes place in xy plane, so remove z position
        _movementBehaviour.DesiredLookatPoint = worldMousePosition;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        _movementBehaviour.Jump();
    }

    private void HandleAttackInput()
    {
        if (_attackBehaviour == null
            || _shootAction == null)
            return;

        if (_shootAction.IsPressed())
            _attackBehaviour.Attack();
    }
}



