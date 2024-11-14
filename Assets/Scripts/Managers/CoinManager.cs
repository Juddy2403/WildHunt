using System;
using UnityEngine;
[Serializable]
public class CoinManager
{
    [SerializeField] private int _coins = 0;
    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            HUD.Instance.UpdateCoins(_coins);
        }
    } 
}
