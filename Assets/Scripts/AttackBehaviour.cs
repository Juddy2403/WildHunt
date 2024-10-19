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
    private enum WeaponType
    {
        Gun,
        Knife
    }
    WeaponType _currentWeapon = WeaponType.Gun;
    void Awake()
    {
        //spawn guns
        if (_gunTemplate != null && _socket != null)
        {
            var gunObject = Instantiate(_gunTemplate, _socket.transform, true);
            gunObject.transform.localPosition = Vector3.zero;
            gunObject.transform.localRotation = Quaternion.identity;
            _weapon = gunObject.GetComponent<BasicWeapon>();
        }
    }

    public void SwitchWeapon()
    {
        if(_weapon) Destroy(_weapon.gameObject);
        switch (_currentWeapon)
        {
            case WeaponType.Gun:
            {
                InstantiateWeaponInSocket(_knifeTemplate);
                _currentWeapon = WeaponType.Knife;
            }
                break;
            case WeaponType.Knife:
            {
                InstantiateWeaponInSocket(_gunTemplate);
                _currentWeapon = WeaponType.Gun;
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
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



