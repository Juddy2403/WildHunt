using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TextPopup : SingletonBase<TextPopup>
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private Label _text = null;
    private void Start()
    {
        _attachedDocument = GetComponent<UIDocument>();
        _attachedDocument.enabled = false;
    }
    public void Display(string text)
    {
        _attachedDocument.enabled = true;
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;

        if (_root == null) return;
        _text = _root.Q<Label>();
        _text.text = text;
        Invoke(nameof(DisableText),1);
    }
    
    private void DisableText()
    {
        _attachedDocument.enabled = false;
    }
}
