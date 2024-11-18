using System;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _gunTemplate = null;
    [SerializeField] private GameObject _knifeTemplate = null;
    [SerializeField] private GameObject _socket = null;
    private BasicWeapon _weapon = null;
    public enum WeaponType
    {
        Gun,
        Knife,
        Empty
    }

    public WeaponType CurrentWeapon { get; private set; } = WeaponType.Gun;

    private void Awake()
    {
        if (GameMaster.Instance.IsIndoors || (name == "Player" && GameMaster.Instance.DayManager.CurrentDay == 0))
        {
            CurrentWeapon = WeaponType.Empty;
            return;
        }
        //spawn guns
        InstantiateWeaponInSocket(_gunTemplate);
    }

    public void SwitchWeapon(WeaponType weaponIndex)
    {
        //destroy the current weapon (if there is one) and spawn the next one
        if(weaponIndex == CurrentWeapon) return;
        if(_weapon) Destroy(_weapon.gameObject);
        switch (weaponIndex)
        {
            case WeaponType.Gun:
            {
                InstantiateWeaponInSocket(_gunTemplate);
                CurrentWeapon = WeaponType.Gun;
            }
                break;
            case WeaponType.Knife: //knife
            {
                InstantiateWeaponInSocket(_knifeTemplate);
                CurrentWeapon = WeaponType.Knife;
            }
                break;
            case WeaponType.Empty: //empty
            {
                CurrentWeapon = WeaponType.Empty;
            }
                break;
            default: throw new ArgumentOutOfRangeException();
        }
    }
    private void InstantiateWeaponInSocket(GameObject weaponTemplate)
    {
        if (weaponTemplate == null || _socket == null) return;
        var weaponObject = Instantiate(weaponTemplate, _socket.transform, true);
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.identity;
        _weapon = weaponObject.GetComponent<BasicWeapon>();
    }
    public void Attack()
    {
        _weapon?.Fire();
    }
}



