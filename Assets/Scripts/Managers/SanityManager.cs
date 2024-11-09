// SanityManager.cs
using UnityEngine;

public class SanityManager
{
    private int _sanity = 100;
    public int Sanity => _sanity;

    public void SanityLost()
    {
        if(_sanity <= 0)
        {
            //resetting it in case a lot of trust is lost at once and it becomes negative
            _sanity = 0;
            return;
        }
        _sanity -= 10;
        //update the HUD
        HUD.Instance.UpdateSanity(_sanity);
    }
}