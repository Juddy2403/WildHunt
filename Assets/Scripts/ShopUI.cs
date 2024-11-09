using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUI : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private Upgrade _hpUpgrade = null;
    private Upgrade _gunUpgrade = null;
    private Upgrade _knifeUpgrade = null;
    private Upgrade _movementUpgrade = null;

    private class Upgrade
    {
        public string _name;
        private Button _button;
        private ProgressBar _progressBar;
        private float _progressBarValue;
        private Label _costLabel;
        private int _costLabelValue;

        public Upgrade(VisualElement root, string name)
        {
            _name = name;
            _button = root.Q<Button>(name + "UpgradeButton");
            _progressBar = root.Q<ProgressBar>(name + "UpgradeBar");
            _costLabel = root.Q<Label>(name + "UpgradePrice");
            _progressBarValue = 0f;
            _costLabelValue = 20;
            _button.clickable.clicked += OnUpgrade;
        }

        public void Update(VisualElement root)
        {
            _button = root.Q<Button>(_name + "UpgradeButton");
            _progressBar = root.Q<ProgressBar>(_name + "UpgradeBar");
            _progressBar.value = _progressBarValue;
            _costLabel = root.Q<Label>(_name + "UpgradePrice");
            _costLabel.text = _costLabelValue.ToString();
            _button.clickable.clicked += OnUpgrade;
        }

        private void OnUpgrade()
        {
            Debug.Log("Upgrading " + _name);
            _progressBar.value += 10f;
            _progressBarValue += 10f;
            _costLabelValue += 10;
            _costLabel.text = _costLabelValue.ToString();
        }
    }

    private void OnEnable()
    {
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;
        if (_hpUpgrade == null) _hpUpgrade = new Upgrade(_root, "Hp");
        else _hpUpgrade.Update(_root);
        if (_gunUpgrade == null) _gunUpgrade = new Upgrade(_root, "Gun");
        else _gunUpgrade.Update(_root);
        if (_knifeUpgrade == null) _knifeUpgrade = new Upgrade(_root, "Knife");
        else _knifeUpgrade.Update(_root);
        if (_movementUpgrade == null) _movementUpgrade = new Upgrade(_root, "Movement");
        else _movementUpgrade.Update(_root);
    }
}