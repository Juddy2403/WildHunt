using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingletonBase<GameMaster>
{
    private int _currentDay = 1;
    private int _creaturesSaved = 0;
    private int _trust = 100;
    private int _sanity = 100;
    private bool _isIndoors = false;
    public int CreaturesSaved { get { return _creaturesSaved; } } 
    public int Trust { get { return _trust; } } 
    public int Sanity { get { return _sanity; } }
    public bool IsIndoors { get { return _isIndoors; } }

    public void SceneChange()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Outside":
                _isIndoors = true;
                SceneManager.LoadScene("Inside");
                break;
            case "Inside":
                _isIndoors = false;
                SceneManager.LoadScene("Outside");
                GameMaster.Instance.DayPassed();
                break;
        }
    }
    public void CreatureSaved()
    {
        ++_creaturesSaved;
        //update the HUD
        HUD.Instance.UpdateCreaturesSaved(_creaturesSaved);
    }
    public void TrustLost()
    {
        if(_trust <= 0) return;
        _trust -= 5;
        //update the HUD
        HUD.Instance.UpdateTrust(_trust);
    }
    public void SanityLost()
    {
        if(_sanity <= 0) return;
        _sanity -= 5;
        //update the HUD
        HUD.Instance.UpdateSanity(_sanity);
    }
    public void DayPassed()
    {
        ++_currentDay;
        //update the HUD
        HUD.Instance.UpdateDay(_currentDay);
    }
}