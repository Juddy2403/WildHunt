using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int _hour = 0;
    private int _minute = 0;

    void Start()
    {
        Invoke(nameof(MinutePassed), 1);
    }

    private void MinutePassed()
    {
        if (_minute < 59) _minute++;
        else
        {
            _minute = 0;
            _hour++;
        }
        HUD.Instance.UpdateTime(_hour, _minute);
        Invoke(nameof(MinutePassed), 1);
    }
}
