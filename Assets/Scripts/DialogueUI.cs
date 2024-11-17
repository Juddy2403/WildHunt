using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private AudioSource _audio = null;
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
        GameMaster.Player.GetComponent<MovementBehaviour>().CanMove = false;
        StartDialogue();
    }

    private void OnDisable()
    {
        GameMaster.Player.GetComponent<MovementBehaviour>().CanMove = true;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if(_text.text == lines[_currentLine]) NextLine();
        else
        {
            _audio.enabled = false;
            StopAllCoroutines();
            _text.text = lines[_currentLine];
        }
    }

    void StartDialogue()
    {
        _currentLine = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        _audio.enabled = true;
        foreach (char c in lines[_currentLine].ToCharArray())
        {
            _text.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        _audio.enabled = false;
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
            _audio.enabled = false;
            //disable the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameMaster.Player.GetComponent<MovementBehaviour>().CanMove = true;
            gameObject.SetActive(false);
        }
    }
}
