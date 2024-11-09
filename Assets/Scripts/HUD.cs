using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUD : SingletonBase<HUD>
{
    private UIDocument _attachedDocument = null;
    private VisualElement _root = null;
    private ProgressBar _healthBar = null;
    private VisualElement _healthBarContainer = null;
    private ProgressBar _trustBar = null;
    private VisualElement _trustBarContainer = null;
    private ProgressBar _sanityBar = null;
    private VisualElement _sanityBarContainer = null;
    private Label _dayNr = null;
    private Label _savedCreatureNr = null;
    private Label _coins = null;

    // Start is called before the first frame update
    void Start()
    {
        //UI
        _attachedDocument = GetComponent<UIDocument>();
        if (_attachedDocument) _root = _attachedDocument.rootVisualElement;

        if (_root == null) return;
        _healthBar = _root.Q<ProgressBar>("PlayerHealthbar"); 
        _healthBarContainer = _healthBar.Q(className: "unity-progress-bar__progress");
        
        _trustBar = _root.Q<ProgressBar>("TrustBar"); 
        _trustBarContainer = _trustBar.Q(className: "unity-progress-bar__progress");
        _trustBarContainer.style.backgroundColor = Color.magenta;
        
        _sanityBar = _root.Q<ProgressBar>("SanityBar"); 
        _sanityBarContainer = _sanityBar.Q(className: "unity-progress-bar__progress");
        _sanityBarContainer.style.backgroundColor = Color.blue;
        
        _dayNr = _root.Q<Label>("DayNumber");
        _savedCreatureNr = _root.Q<Label>("CreaturesSaved");
        _coins = _root.Q<Label>("Coins");

        HookHealthEvent();
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
        _healthBar.title = $"hp {currentHealth}/{startHealth}";
        
        //change the healthbarContainer color from green to red based on the health percentage
        if (_healthBarContainer == null) return;
        if (currentHealth / startHealth < 0.25f) _healthBarContainer.style.backgroundColor = Color.red;
        else if (currentHealth / startHealth < 0.5f) _healthBarContainer.style.backgroundColor = Color.yellow;
        else _healthBarContainer.style.backgroundColor = Color.green;
    }
    
    public void UpdateTrust(int trust)
    {
        if (_trustBar == null) return;
        _trustBar.value = trust;
        _trustBar.title = $"trust {trust}/{100}";
    }
    public void UpdateSanity(int sanity)
    {
        if (_sanityBar == null) return;
        _sanityBar.value = sanity;
        _sanityBar.title = $"sanity {sanity}/{100}";
    }
    public void UpdateCoins(int coins)
    {
        if (_coins == null) return;
        _coins.text = $"Coins: {coins}";
    }
}