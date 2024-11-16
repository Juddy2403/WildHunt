using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UIInstanceManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private GameObject _UIObj = null;
    [SerializeField] private string _UIObjName;
    [SerializeField] private string _textToDisplay;
    [SerializeField] private bool _hideCursor = true;
    private InputAction _interactAction;
    private bool _isOnBox = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!_inputAsset) return;

        if(_UIObjName == "ShopUI")
        {
            _UIObj = ShopUI.Instance.gameObject;
        }
        _UIObj.SetActive(false);

        //Bind the actions to the input asset
        _interactAction = _inputAsset.FindActionMap("Gameplay").FindAction("Interact");

        //we bind a callback to it instead of continiously monitoring input
        _interactAction.performed += HandleInteraction;
    }

    private void OnDestroy()
    {
        _interactAction.performed -= HandleInteraction;
    }
    // Update is called once per frame
    private void HandleInteraction(InputAction.CallbackContext obj)
    {
        if (!_isOnBox) return;
        _UIObj.SetActive(!_UIObj.activeSelf);
        if(!_hideCursor) return;
        if (_UIObj.activeSelf)
        {
            //enable the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            //disable the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameMaster.Player)
        {
            _isOnBox = true;
            TextPopup.Instance.Display(_textToDisplay);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != GameMaster.Player) return;
        _isOnBox = false;
        _UIObj.SetActive(false);
    }
}