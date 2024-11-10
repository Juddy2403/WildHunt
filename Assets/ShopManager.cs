using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputAsset;
    private GameObject _shopUI = null;
    private InputAction _interactAction;
    private bool _isOnShop = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!_inputAsset) return;

        _shopUI = FindObjectOfType<ShopUI>().gameObject;
        _shopUI.SetActive(false);

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
        if (!_isOnShop) return;
        _shopUI.SetActive(!_shopUI.activeSelf);
        if (_shopUI.activeSelf)
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
            _isOnShop = true;
            TextPopup.Instance.Display("Press E to open shop");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != GameMaster.Player) return;
        _isOnShop = false;
        _shopUI.SetActive(false);
    }
}
