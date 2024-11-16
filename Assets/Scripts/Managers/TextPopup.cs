using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TextPopup : SingletonBase<TextPopup>
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private Label _text = null;
    private const float _fadeTime = 0.5f;

    private void Start()
    {
        _attachedDocument = GetComponent<UIDocument>();
        _attachedDocument.enabled = false;
    }
    public void Display(string text, float fadeAfter = 1)
    {
        _attachedDocument.enabled = true;
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;
        if (_root == null) return;
        StopAllCoroutines();
        _text = _root.Q<Label>();
        _text.style.opacity = 1;
        _text.text = text;
        Invoke(nameof(FadeText),fadeAfter);
    }
    private void FadeText()
    {
        StartCoroutine(FadeTextCoroutine());
    }
    private IEnumerator FadeTextCoroutine()
    {
        float time = _fadeTime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            var normalizedTime = Mathf.Clamp01(time / _fadeTime);
            _text.style.opacity = normalizedTime;
            yield return new WaitForEndOfFrame();//this loop will continue next frame until the while loop has finished
        }
        //ensure we are the starting color again at the end if we would not exactly hit it due to rounding
        _attachedDocument.enabled = false;

    }
}
