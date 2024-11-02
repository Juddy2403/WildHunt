using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUD : SingletonBase<HUD>
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private ProgressBar _healthBar = null;
    private VisualElement _healthBarContainer = null;
    private Label _dayNr = null;
    private Label _savedCreatureNr = null;

    // Start is called before the first frame update
    void Start()
    {
        //UI
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;

        if (_root == null) return;
        _healthBar = _root.Q<ProgressBar>(); 
        _healthBarContainer = _healthBar.Q(className: "unity-progress-bar__progress");
        
        _dayNr = _root.Q<Label>("DayNumber");
        _savedCreatureNr = _root.Q<Label>("CreaturesSaved");

        HookHealthEvent();
    }

    private void HookHealthEvent()
    {
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
    
    public void UpdateCreaturesSaved(int creaturesSaved)
    {
        if (_savedCreatureNr == null) return;
        _savedCreatureNr.text = $"Creatures saved: {creaturesSaved}";
    }
    private void UpdateHealth(float startHealth, float currentHealth)
    {
        if (_healthBar == null) return;

        _healthBar.value = (currentHealth / startHealth) * 100.0f;
        _healthBar.title = string.Format("{0}/{1}", currentHealth, startHealth);
        
        //change the healthbarContainer color from green to red based on the health percentage
        if (_healthBarContainer == null) return;
        if (currentHealth / startHealth < 0.25f) _healthBarContainer.style.backgroundColor = Color.red;
        else if (currentHealth / startHealth < 0.5f) _healthBarContainer.style.backgroundColor = Color.yellow;
        else _healthBarContainer.style.backgroundColor = Color.green;
    }
}