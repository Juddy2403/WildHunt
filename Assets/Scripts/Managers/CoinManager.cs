
public class CoinManager
{
    private int _coins = 0;
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
