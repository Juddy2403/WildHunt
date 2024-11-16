using UnityEngine;

public class DayManager
{
    private int _currentDay = 0;
    public int CurrentDay => _currentDay;

    public void DayPassed()
    {
        _currentDay++;
        HUD.Instance.UpdateDay(_currentDay);
    }
}