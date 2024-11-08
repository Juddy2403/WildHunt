using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class Shop : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputAsset;
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private InputAction _interactAction;
    private bool _isOnShop = false;
    
    private void Start()
    {
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;
        _attachedDocument.enabled = false;
        
        if (!_inputAsset) return;

        //Bind the actions to the input asset
        _interactAction = _inputAsset.FindActionMap("Gameplay").FindAction("Interact");

        //we bind a callback to it instead of continiously monitoring input
        _interactAction.performed += HandleInteraction;
    }

    private void HandleInteraction(InputAction.CallbackContext obj)
    {
        if (!_isOnShop) return;
        _attachedDocument.enabled = !_attachedDocument.enabled;
        if (_attachedDocument.enabled)
        {
            //enable the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == GameMaster.Player)
        {
            _isOnShop = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == GameMaster.Player)
        {
            _isOnShop = false;
            _attachedDocument.enabled = false;
        }
    }
}
