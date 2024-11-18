using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    WeaponType _currentWeapon = WeaponType.Gun;
    public WeaponType CurrentWeapon { get { return _currentWeapon; } }
    void Awake()
    {
        if (GameMaster.Instance.IsIndoors || GameMaster.Instance.DayManager.CurrentDay == 0)
        {
            _currentWeapon = WeaponType.Empty;
            return;
        }
        //spawn guns
        InstantiateWeaponInSocket(_gunTemplate);
    }

    public void SwitchWeapon(WeaponType weaponIndex)
    {
        //destroy the current weapon (if there is one) and spawn the next one
        if(weaponIndex == _currentWeapon) return;
        if(_weapon) Destroy(_weapon.gameObject);
        switch (weaponIndex)
        {
            case WeaponType.Gun:
            {
                InstantiateWeaponInSocket(_gunTemplate);
                _currentWeapon = WeaponType.Gun;
            }
                break;
            case WeaponType.Knife: //knife
            {
                InstantiateWeaponInSocket(_knifeTemplate);
                _currentWeapon = WeaponType.Knife;
            }
                break;
            case WeaponType.Empty: //empty
            {
                _currentWeapon = WeaponType.Empty;
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



