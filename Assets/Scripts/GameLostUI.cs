using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameLostUI : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private Button _replayButton = null;
    void OnEnable()
    {
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;
        _replayButton = _root.Q<Button>("PlayAgainButton");
        _replayButton.clickable.clicked += OnReplay;
    }

    // Update is called once per frame
    void OnReplay()
    {
        GameMaster.Instance.TriggerReloadGame();
    }
}
