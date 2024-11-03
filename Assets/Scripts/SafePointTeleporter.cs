using UnityEngine;
using UnityEngine.InputSystem;

public class SafePointTeleporter : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputAsset;
    private InputAction _interactAction;
    private bool _isOnSafePoint = false;
    void Awake()
    {
        if (!_inputAsset) return;

        //Bind the actions to the input asset
        _interactAction = _inputAsset.FindActionMap("Gameplay").FindAction("Interact");

        //we bind a callback to it instead of continiously monitoring input
        _interactAction.performed += HandleInteraction;
    }
    void OnDestroy()
    {
        _interactAction.performed -= HandleInteraction;
    }
    void HandleInteraction(InputAction.CallbackContext context)
    {
        if (!_isOnSafePoint) return;
        GameMaster.Instance.SceneChange();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "SafePointCollider") _isOnSafePoint = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "SafePointCollider") _isOnSafePoint = false;
    }
}
