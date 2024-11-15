using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueUI : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private Label _text = null;
    public string[] lines;
    public float textSpeed;
    private int _currentLine = 0;

    // Start is called before the first frame update
    void OnEnable()
    {
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;
        _text = _root.Q<Label>("DialogueText");
        _text.text = "";
        StartDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(_text.text == lines[_currentLine]) NextLine();
            else
            {
                StopCoroutine(TypeLine());
                _text.text = lines[_currentLine];
            }
        }
    }

    void StartDialogue()
    {
        _currentLine = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in lines[_currentLine].ToCharArray())
        {
            _text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if(_currentLine < lines.Length - 1)
        {
            _currentLine++;
            _text.text = "";
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
