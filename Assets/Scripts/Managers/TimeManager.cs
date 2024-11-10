using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int hour = 0;
    private int minute = 0;

    void Start()
    {
        Invoke(nameof(MinutePassed), 1);
    }

    private void MinutePassed()
    {
        if (minute < 59) minute++;
        else
        {
            minute = 0;
            hour++;
        }
        HUD.Instance.UpdateTime(hour, minute);
        Invoke(nameof(MinutePassed), 1);
    }
}
