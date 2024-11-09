// DayManager.cs
using UnityEngine;

public class DayManager
{
    private int _currentDay = 1;
    public int CurrentDay => _currentDay;

    public void DayPassed()
    {
        _currentDay++;
        HUD.Instance.UpdateDay(_currentDay);
    }
}