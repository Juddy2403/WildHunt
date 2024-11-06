using System.Collections;
using System.Collections.Generic;
using BigRookGames.Weapons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BasicWeapon : MonoBehaviour
{
    [SerializeField] private GameObject _bulletTemplate = null;
    [SerializeField] private float _fireRate = 25.0f;
    [SerializeField] private GameObject _shootVfx = null;
    [SerializeField] private Transform _fireSocket = null;
    [SerializeField] private UnityEvent _onFireEvent;
    private bool _triggerPulled = false;
    private float _fireTimer = 0.0f;

    private void Update()
    {
        //handle the countdown of the fire timer
        if (_fireTimer > 0.0f) _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0.0f && _triggerPulled) FireProjectile();

        //the trigger will release by itself, 
        //if we still are firing, we will receive new fire input
        _triggerPulled = false;
    }

    private void FireProjectile()
    {
        //no bullet to fire
        if (!_bulletTemplate) return;
        
        if(_shootVfx) Instantiate(_shootVfx, _fireSocket.transform);

        //set the time so we respect the firerate
        _fireTimer += 1.0f / _fireRate;
        _onFireEvent?.Invoke();

        Instantiate(_bulletTemplate, _fireSocket.position, _fireSocket.rotation);
    }

    public void Fire()
    {
        _triggerPulled = true;
    }
}