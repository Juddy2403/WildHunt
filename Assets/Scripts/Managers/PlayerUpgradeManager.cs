using UnityEngine;

public class PlayerUpgradeManager
{
    private int _healthIncrease = 0;
    private int _gunDamageIncrease = 0;
    public int GunDamageIncrease => _gunDamageIncrease;
    private int _knifeDamageIncrease = 0;
    public int KnifeDamageIncrease => _knifeDamageIncrease;
    private int _movementIncrease = 0;
    public int MovementIncrease => _movementIncrease;

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
                _gunDamageIncrease += 5;
                break;
            case "Knife":
                //UpgradeKnife();
                _knifeDamageIncrease += 5;
                break;
            case "Movement":
                _movementIncrease += 1;
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