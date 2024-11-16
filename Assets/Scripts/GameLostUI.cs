using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameLostUI : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private Button _replayButton = null;
    private Label _savedCreaturesLabel = null;

    void OnEnable()
    {
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;
        _replayButton = _root.Q<Button>("PlayAgainButton");
        _replayButton.clickable.clicked += OnReplay;
        _savedCreaturesLabel = _root.Q<Label>("Saved");
        _savedCreaturesLabel.text = $"Saved: {GameMaster.Instance.CreatureManager.CreaturesSaved}/{GameMaster.Instance.CreatureQuota}";
    }

    // Update is called once per frame
    void OnReplay()
    {
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForEndOfFrame();
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.name == name) continue;
            Destroy(obj);
        }

        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(0);
    }
}