using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikazeCharacter : BasicCharacter
{
    private GameObject _playerTarget = null;
    private GameObject _currentTarget = null;
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _playerFollowRange = 10.0f;
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
        //if enemy doesnt follow creature and is close enough to player
        if (!_currentTarget && (transform.position - _playerTarget.transform.position).sqrMagnitude <
            _playerFollowRange * _playerFollowRange)
        {
            _currentTarget = _playerTarget;
        }
        //if enemy follows creature but player is very close
        else if ((transform.position - _playerTarget.transform.position).sqrMagnitude < _playerFollowRange &&
                 _currentTarget != _playerTarget)
        {
            _currentTarget = _playerTarget;
        }

        if (_currentTarget) _movementBehaviour.Target = _currentTarget.transform;
        else _movementBehaviour.Target = null;
    }

    void HandleAttacking()
    {
        if (_hasAttacked) return;
        if (!_attackBehaviour) return;
        if (!_currentTarget) return;

        //if we are in range of the player, fire our weapon, 
        if ((transform.position - _currentTarget.transform.position).sqrMagnitude < _attackRange * _attackRange)
        {
            _hasAttacked = true;
            _attackBehaviour.Attack();
            if (_attackVFXTemplate) Instantiate(_attackVFXTemplate, transform.position, transform.rotation);
            _movementBehaviour.PushBackwards(-transform.forward);
            //turn has attacked off after a delay
            Invoke("ResetAttack", 1.0f);
        }
    }

    private void ResetAttack()
    {
        _hasAttacked = false;
    }
    
    public void CreatureDetected(GameObject creature)
    {
        //if we are not already targeting a creature, follow this one
        if (!_currentTarget || !_currentTarget.CompareTag("Creature"))
        {
            _currentTarget = creature;
            _movementBehaviour.Target = _currentTarget.transform;
        }
    }

    public void TargetDestroyed()
    {
        _currentTarget = null;
        _movementBehaviour.Target = null;
    }
}