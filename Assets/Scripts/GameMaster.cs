using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingletonBase<GameMaster>
{
    private int _currentDay = 1;
    private int _creaturesSaved = 0;

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    public void CreatureSaved()
    {
        ++_creaturesSaved;
        //update the HUD
        HUD.Instance.UpdateCreaturesSaved(_creaturesSaved);
    }
    public void DayPassed()
    {
        ++_currentDay;
        //update the HUD
        HUD.Instance.UpdateDay(_currentDay);
    }
}