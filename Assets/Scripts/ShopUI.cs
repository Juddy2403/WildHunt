using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUI : MonoBehaviour
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private Upgrade _hpUpgrade;
    private Upgrade _gunUpgrade;
    private Upgrade _knifeUpgrade;
    private Upgrade _movementUpgrade;
    
    private struct Upgrade
    {
        public string _name;
        private Button _button;
        private ProgressBar _progressBar;
        private Label _costLabel;

        public Upgrade(VisualElement root,string name)
        {
            _name = name;
            _button = root.Q<Button>(name + "UpgradeButton");
            _progressBar = root.Q<ProgressBar>(name + "UpgradeBar");
            _costLabel = root.Q<Label>(name + "UpgradePrice");
            _button.clickable.clicked += OnUpgrade;
        }
        
        private void OnUpgrade()
        {
            Debug.Log("Upgrading " + _name);
            _progressBar.value += 10f;
            int cost = Convert.ToInt32(_costLabel.text);
            cost += 10;
            _costLabel.text = cost.ToString();
        }
    }
    private void OnEnable()
    {
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;
        _hpUpgrade = new Upgrade(_root, "Hp");
        _gunUpgrade = new Upgrade(_root, "Gun");
        _knifeUpgrade = new Upgrade(_root, "Knife");
        _movementUpgrade = new Upgrade(_root, "Movement");
        
    }

    private void OnHpUpgrade()
    {
        //upgrade the player's health
        Debug.Log("Upgrading health");
    }
    
}
