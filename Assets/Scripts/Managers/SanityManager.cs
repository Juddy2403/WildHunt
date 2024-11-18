public class SanityManager
{
    public int Sanity { get; private set; } = 100;

    public void SanityLost()
    {
        if(Sanity <= 0)
        {
            //resetting it in case a lot of trust is lost at once and it becomes negative
            Sanity = 0;
            return;
        }
        Sanity -= 10;
        //update the HUD
        HUD.Instance.UpdateSanity(Sanity);
    }
}