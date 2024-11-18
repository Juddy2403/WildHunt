public class PlayerUpgradeManager
{
    private int _healthIncrease = 0;
    public int GunDamageIncrease { get; private set; } = 0;
    public int KnifeDamageIncrease { get; private set; } = 0;
    public int MovementIncrease { get; private set; } = 0;

    public void Upgrade(string name)
    {
        switch (name)
        {
            case "Hp":
                // UpgradeHp();
                _healthIncrease += 20;
                GameMaster.Player.GetComponent<Health>().StartHealth += 20;
                break;
            case "Gun":
                //UpgradeGun();
                GunDamageIncrease += 5;
                break;
            case "Knife":
                //UpgradeKnife();
                KnifeDamageIncrease += 5;
                break;
            case "Movement":
                MovementIncrease += 1;
                GameMaster.Player.GetComponent<PlayerCharacter>().MovementSpeed += 1;
                break;
        }
    }

    public void ApplyHealthIncrease()
    {
        if(GameMaster.Player == null) return;
        GameMaster.Player.GetComponent<Health>().StartHealth += _healthIncrease;
    }
}