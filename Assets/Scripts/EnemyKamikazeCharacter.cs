using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikazeCharacter : BasicCharacter
{
    private GameObject _playerTarget = null;
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] GameObject _attackVFXTemplate = null;
    private bool _hasAttacked = false;


    private void Start()
    {
        //expensive method, use with caution
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        if (player) _playerTarget = player.gameObject;
    }

    private void Update()
    {
        HandleMovement();
        HandleAttacking();
    }

    void HandleMovement()
    {
        if (!_movementBehaviour) return;
        _movementBehaviour.Target = _playerTarget;
    }

    void HandleAttacking()
    {
        if (_hasAttacked) return;
        if (!_attackBehaviour) return;
        if (!_playerTarget) return;

        //if we are in range of the player, fire our weapon, 
        //use sqr magnitude when comparing ranges as it is more efficient
        if ((transform.position - _playerTarget.transform.position).sqrMagnitude < _attackRange * _attackRange)
        {
            _hasAttacked = true;
            _attackBehaviour.Attack();
            if (_attackVFXTemplate) Instantiate(_attackVFXTemplate, transform.position, transform.rotation);
            _movementBehaviour.PushBackwards();
            //turn has attacked off after a delay
            Invoke("ResetAttack", 1.0f);
        }
    }
    
    void ResetAttack()
    {
        _hasAttacked = false;
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}

