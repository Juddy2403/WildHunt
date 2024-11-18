public class TrustManager
{
    public int Trust { get; private set; } = 100;

    public void TrustLost(int amount)
    {
        if(Trust <= 0)
        {
            //resetting it in case a lot of trust is lost at once and it becomes negative
            Trust = 0;
            return;
        }
        Trust -= amount;
        //update the HUD
        HUD.Instance.UpdateTrust(Trust);
    }
}