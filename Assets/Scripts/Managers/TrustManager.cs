using UnityEngine;

public class TrustManager
{
    private int _trust = 100;
    public int Trust => _trust;

    public void TrustLost(int amount)
    {
        if(_trust <= 0)
        {
            //resetting it in case a lot of trust is lost at once and it becomes negative
            _trust = 0;
            return;
        }
        _trust -= amount;
        //update the HUD
        HUD.Instance.UpdateTrust(_trust);
    }
}