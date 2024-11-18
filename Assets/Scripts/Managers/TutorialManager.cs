using System;
using UnityEngine;

public class TutorialManager
{
    private GameObject _enteredRadius = null;
    private GameObject _enteredRadiusWithWeapon= null;
    private GameObject _murderSeen = null;
    private GameObject _murderedWithGun = null;
    private bool _hasEnteredRadiusBefore = false;
    private bool _hasEnteredRadiusWithWeaponBefore = false;
    private bool _hasMurderedBefore = false;
    private bool _hasMurderedWithGunBefore = false;

    public void OnGoingInside()
    {
        _enteredRadius = GameObject.Find("EnterRadius");
        _enteredRadiusWithWeapon = GameObject.Find("EnterRadiusWithWeapon");
        _murderSeen = GameObject.Find("MurderSeen");
        _murderedWithGun = GameObject.Find("MurderedWithGun");
        _enteredRadius.SetActive(false);
        _enteredRadiusWithWeapon.SetActive(false);
        _murderSeen.SetActive(false);
        _murderedWithGun.SetActive(false);
    }
    public void EnteredRadius()
    {
        if (_hasEnteredRadiusBefore) return;
        _enteredRadius.SetActive(true);
        _hasEnteredRadiusBefore = true;
    }
    
    public void EnteredRadiusWithWeapon()
    {
        if (_hasEnteredRadiusWithWeaponBefore) return;
        _enteredRadiusWithWeapon.SetActive(true);
        _hasEnteredRadiusWithWeaponBefore = true;
    }
    
    public void MurderSeen()
    {
        if (_hasMurderedBefore) return;
        _murderSeen.SetActive(true);
        _hasMurderedBefore = true;
    }
    
    public void MurderedWithGun()
    {
        if (_hasMurderedWithGunBefore) return;
        _murderedWithGun.SetActive(true);
        _hasMurderedWithGunBefore = true;
    }
    
}
