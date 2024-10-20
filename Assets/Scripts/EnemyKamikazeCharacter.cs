using System;
using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyKamikazeCharacter : BasicCharacter
{
    private GameObject _playerTarget = null;
    private GameObject _currentTarget = null;
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _targetFollowRange = 10.0f;
    [SerializeField] GameObject _attackVFXTemplate = null;
    private NavMeshMovementBehaviour _navMovementBehaviour;
    private bool _hasAttacked = false;

    private void Start()
    {
        //expensive method, use with caution
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        if (player) _playerTarget = player.gameObject;
        _navMovementBehaviour = GetComponent<NavMeshMovementBehaviour>();
    }

    private void Update()
    {
        HandleMovement();
        HandleAttacking();
    }

    void HandleMovement()
    {
        if (!_movementBehaviour) return;
        //if enemy doesnt follow creature and is close enough to player or if enemy follows creature but player is very close
        if ((!_currentTarget && IsPlayerInRange()) || (IsPlayerVeryClose() && _currentTarget != _playerTarget))
        {
            _currentTarget = _playerTarget;
            _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, _playerTarget.transform));
        }
        else if (!_currentTarget) _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
    }

    void HandleAttacking()
    {
        if (_hasAttacked) return; if (!_attackBehaviour) return; if (!_currentTarget) return;

        //if we are in range of the player, fire our weapon, 
        if (IsPlayerInAttackRange())
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
        if (_currentTarget && _currentTarget.CompareTag("Creature")) return;
        _currentTarget = creature;
        _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, creature.transform));
    }
    public void CreatureLost(GameObject creature)
    {
        if (_currentTarget != creature) return;
        _currentTarget = null;
        _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
    }

    private bool IsPlayerVeryClose()
    {
        return (transform.position - _playerTarget.transform.position).sqrMagnitude < _targetFollowRange;
    }
    private bool IsPlayerInRange()
    {
        return (transform.position - _playerTarget.transform.position).sqrMagnitude < _targetFollowRange * _targetFollowRange;
    }
    private bool IsPlayerInAttackRange()
    {
        return (transform.position - _currentTarget.transform.position).sqrMagnitude < _attackRange * _attackRange;
    }
}