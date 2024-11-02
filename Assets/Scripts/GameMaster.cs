using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SingletonBase<GameMaster>
{
    private int _currentDay = 1;

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SafePointTeleporter safePointTeleporter = FindObjectOfType<SafePointTeleporter>();
        if (safePointTeleporter == null) return;
        // hook to monitor changes
        safePointTeleporter.OnSafePointExited += DayPassed;
    }

    void DayPassed()
    {
        ++_currentDay;
        //update the HUD
        HUD hud = FindObjectOfType<HUD>();
        hud?.UpdateDay(_currentDay);
    }
}