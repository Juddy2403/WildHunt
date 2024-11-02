using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : SingletonBase<HUD>
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private ProgressBar _healthbar = null;
    private VisualElement _healthbarContainer = null;
    private Label _dayNr = null;

    // Start is called before the first frame update
    void Start()
    {
        //UI
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;

        if (_root == null) return;
        _healthbar = _root.Q<ProgressBar>(); 
        _healthbarContainer = _healthbar.Q(className: "unity-progress-bar__progress");
        
        _dayNr = _root.Q<Label>("DayNumber");
        _dayNr.text = "Day 1";

        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        if (player == null) return;
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth)
        {
            // initialize
            UpdateHealth(playerHealth.StartHealth, playerHealth.CurrentHealth);
            // hook to monitor changes
            playerHealth.OnHealthChanged += UpdateHealth;
        }
    }

    public void UpdateDay(int day)
    {
        if (_dayNr == null) return;
        _dayNr.text = $"Day {day}";
    }
    private void UpdateHealth(float startHealth, float currentHealth)
    {
        if (_healthbar == null) return;

        _healthbar.value = (currentHealth / startHealth) * 100.0f;
        _healthbar.title = string.Format("{0}/{1}", currentHealth, startHealth);
        
        //change the healthbarContainer color from green to red based on the health percentage
        if (_healthbarContainer == null) return;
        if (currentHealth / startHealth < 0.25f) _healthbarContainer.style.backgroundColor = Color.red;
        else if (currentHealth / startHealth < 0.5f) _healthbarContainer.style.backgroundColor = Color.yellow;
        else _healthbarContainer.style.backgroundColor = Color.green;
    }
}