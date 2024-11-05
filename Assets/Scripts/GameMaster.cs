using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingletonBase<GameMaster>
{
    private int _currentDay = 1;
    private int _creaturesSaved = 0;
    private int _trust = 100;
    private int _sanity = 100;
    private bool _isIndoors = false;
    public static GameObject Player { get; set; } = null;
    public int CreaturesSaved { get { return _creaturesSaved; } } 
    public int Trust { get { return _trust; } } 
    public int Sanity { get { return _sanity; } }
    public bool IsIndoors { get { return _isIndoors; } }
    
    private float _waveStartFrequency = 30.0f;
    private float _waveEndFrequency = 15.0f;
    private int _monsterMinSpawnCount = 5;
    private int _monsterMaxSpawnCount = 20;
    public float WaveStartFrequency { get { return _waveStartFrequency; } }
    public float WaveEndFrequency { get { return _waveEndFrequency; } }
    public int MonsterMinSpawnCount { get { return _monsterMinSpawnCount; } }
    public int MonsterMaxSpawnCount { get { return _monsterMaxSpawnCount; } }

    public void SceneChange()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Outside":
                _isIndoors = true;
                SceneManager.LoadScene("Inside");
                RunAwayCreatures();
                break;
            case "Inside":
                _isIndoors = false;
                SceneManager.LoadScene("Outside");
                DayPassed();
                break;
        }
    }

    private void RunAwayCreatures()
    {
        if (_trust < 50)
        {
            float baseChance = (50 - _trust) / 50.0f; // Base chance based on trust level
            int maxCreaturesToLeave = Mathf.FloorToInt(_creaturesSaved / 2.0f); // Maximum number of creatures that can leave (not more than half)
            int creaturesToLeave = 0;

            for (int i = 0; i < _creaturesSaved; i++)
            {
                if (UnityEngine.Random.value < baseChance)
                {
                    creaturesToLeave++;
                    if (creaturesToLeave >= maxCreaturesToLeave)
                    {
                        break; // Stop if we reach the maximum number of creatures that can leave
                    }
                }
            }
            Debug.Log($"Creatures to leave: {creaturesToLeave}");
            _creaturesSaved = Mathf.Max(0, _creaturesSaved - creaturesToLeave); // Decrease the number of saved creatures
            HUD.Instance.UpdateCreaturesSaved(_creaturesSaved); // Update the HUD
        }
    }
    public void TriggerGameOver()
    {
        StartCoroutine(ReloadScene());        
    }
    
    public void CreatureMurdered()
    {
        HUD.Instance.UpdateCreaturesSaved(--_creaturesSaved); // Update the HUD
    }
    private static IEnumerator ReloadScene()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(0);
    }
    public void CreatureSaved()
    {
        ++_creaturesSaved;
        //update the HUD
        HUD.Instance.UpdateCreaturesSaved(_creaturesSaved);
    }
    public void TrustLost(int amount)
    {
        if(_trust <= 0) return;
        _trust -= amount;
        //update the HUD
        HUD.Instance.UpdateTrust(_trust);
    }
    public void SanityLost()
    {
        if(_sanity <= 0) return;
        _sanity -= 10;
        //update the HUD
        HUD.Instance.UpdateSanity(_sanity);
    }

    private void DayPassed()
    {
        ++_currentDay;
        //update the HUD
        HUD.Instance.UpdateDay(_currentDay);
    }
}