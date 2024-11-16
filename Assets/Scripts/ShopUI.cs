using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class ShopUI : SingletonBase<ShopUI>
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private Upgrade _hpUpgrade = null;
    private Upgrade _gunUpgrade = null;
    private Upgrade _knifeUpgrade = null;
    private Upgrade _movementUpgrade = null;
    private Button _exitButton;

    private class Upgrade
    {
        private string _name;
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
            if(_progressBarValue >= 100f) return;
            if(GameMaster.Instance.CoinManager.Coins < _costLabelValue)
            {
                TextPopup.Instance.Display("Not enough coins");
                return;
            }
            GameMaster.Instance.CoinManager.Coins -= _costLabelValue;
            Debug.Log("Upgrading " + _name);
            _progressBar.value += 20f;
            _progressBarValue += 20f;
            _costLabelValue += 20;
            _costLabel.text = _costLabelValue.ToString();
            GameMaster.Instance.PlayerUpgradeManager.Upgrade(_name);
        }
    }

    private void OnEnable()
    {
        gameObject?.SetActive(GameMaster.Instance.IsIndoors);
        if(!gameObject.activeSelf) return;
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
        _exitButton = _root.Q<Button>("ExitButton");
        if (_exitButton != null) _exitButton.clickable.clicked += OnExit;
    }

    private void OnExit()
    {
        //disable the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameMaster.Player.GetComponent<MovementBehaviour>().CanMove = true;
        gameObject.SetActive(false);
    }

    // protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if(GameMaster.Instance.IsIndoors) gameObject?.SetActive(true);
    // }
}