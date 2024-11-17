using System;
using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyKamikazeCharacter : BasicCharacter
{
    [SerializeField] private GameObject _currentTarget = null;
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _targetFollowRange = 20.0f;
    [SerializeField] GameObject _attackVFXTemplate = null;
    [SerializeField] private AudioSource _growlSound = null;
    [SerializeField] private AudioSource _roarSound = null;
    private NavMeshMovementBehaviour _navMovementBehaviour;
    private bool _hasAttacked = false;

    private void Start()
    {
        _navMovementBehaviour = GetComponent<NavMeshMovementBehaviour>();
        var currentDay = GameMaster.Instance.DayManager.CurrentDay;
        if(currentDay > 1)
        {
            //movement speed gets reset when in idle, so we set the state to null first to prevent that
            _navMovementBehaviour.SetState(null);
            _navMovementBehaviour.MovementSpeed += (currentDay - 1) * 2.5f;
            _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
        }
        Invoke(nameof(GrowlSound),UnityEngine.Random.Range(2.0f, 10.0f));
    }

    private void GrowlSound()
    {
        if (_growlSound) _growlSound.Play();
        Invoke(nameof(GrowlSound), UnityEngine.Random.Range(5.0f, 15.0f));
    }
    private void FixedUpdate()
    {
        HandleMovement();
        HandleAttacking();
    }

    private void HandleMovement()
    {
        if (!_movementBehaviour || !GameMaster.Player) return;

        if (ShouldFollowPlayer()) FollowPlayer();
        else if (!_currentTarget) SetIdleState();
    }

    private bool ShouldFollowPlayer()
    {
        //follow player if we have no target and the player is in range,
        //or if the player is very close and we are not already following them
        return (!_currentTarget && IsPlayerInRange()) || (IsPlayerVeryClose() && _currentTarget != GameMaster.Player);
    }

    private void FollowPlayer()
    {
        _currentTarget = GameMaster.Player;
        _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, _currentTarget.transform));
    }

    private void SetIdleState()
    {
        _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
    }

    void HandleAttacking()
    {
        if (_hasAttacked || !_attackBehaviour || !_currentTarget) return;

        //if we are in range of the player, fire our weapon, 
        if (IsPlayerInAttackRange())
        {
            _hasAttacked = true;
            _attackBehaviour.Attack();
            if (_attackVFXTemplate) Instantiate(_attackVFXTemplate, transform.position, transform.rotation);
            _movementBehaviour.PushBackwards(-transform.forward);
            //can only attack again after a delay
            Invoke(nameof(ResetAttack), 1.0f);
        }
    }

    private void ResetAttack()
    {
        _hasAttacked = false;
    }

    public void CreatureDetected(GameObject creature)
    {
        if (_roarSound) _roarSound.Play();
        //if we are not already targeting a creature, follow this one
        if (_currentTarget && _currentTarget.CompareTag("Creature")) return;
        if (_currentTarget == GameMaster.Player && IsPlayerInRange()) return;
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
        return (transform.position - GameMaster.Player.transform.position).sqrMagnitude < _targetFollowRange;
    }

    private bool IsPlayerInRange()
    {
        return (transform.position - GameMaster.Player.transform.position).sqrMagnitude < _targetFollowRange * _targetFollowRange;
    }

    private bool IsPlayerInAttackRange()
    {
        return (transform.position - _currentTarget.transform.position).sqrMagnitude < _attackRange * _attackRange;
    }
}