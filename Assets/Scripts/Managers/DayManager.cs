public class DayManager
{
    public int CurrentDay { get; private set; } = 0;

    public void DayPassed()
    {
        CurrentDay++;
        HUD.Instance.UpdateDay(CurrentDay);
    }
}