using System;
using System.Collections.Generic;
using Movement;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureAI : BasicCharacter
{
    private bool _isAlive = true;
    private bool _detectedSafePoint = false;
    private const float _followRange = 10.0f;
    private const float _idleRange = 4.0f;
    private bool _areMonstersClose = false;
    private NavMeshMovementBehaviour _navMovementBehaviour;

    void Start()
    {
        _navMovementBehaviour = GetComponent<NavMeshMovementBehaviour>();
        _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
    }

    private void OnEnable()
    {
        if (GameMaster.Instance.IsIndoors)
        {
            gameObject.GetComponent<Health>().StartHealth = 10;
        }
    }

    void FixedUpdate()
    {
        if(!GameMaster.Instance.Player) return;
        if(GameMaster.Instance.IsIndoors) return;
        if(_detectedSafePoint) return;
        
        if (_areMonstersClose) _navMovementBehaviour.SetState(new RunState(_navMovementBehaviour));
        else if (IsPlayerInFollowRange() && IsPlayerNotTooClose())
            _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, GameMaster.Instance.Player.transform));
        else if (!IsPlayerNotTooClose()) _navMovementBehaviour.SetState(null);
        else _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
        _areMonstersClose = false;
    }

    private void OnDestroy()
    {
        //on trigger exit is not called when obj destroyed, so i do it here
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius);
        foreach (var collider in colliders) OnTriggerExit(collider);
    }

    private bool IsPlayerNotTooClose()
    {
        return (transform.position - GameMaster.Instance.Player.transform.position).sqrMagnitude > _idleRange * _idleRange;
    }

    private bool IsPlayerInFollowRange()
    {
        return (transform.position - GameMaster.Instance.Player.transform.position).sqrMagnitude < _followRange * _followRange;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "KamikazeEnemy")
        {
            _areMonstersClose = true;
            EnemyKamikazeCharacter enemyKamikazeCharacter = other.GetComponent<EnemyKamikazeCharacter>();
            if (enemyKamikazeCharacter) enemyKamikazeCharacter.CreatureDetected(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(GameMaster.Instance.IsIndoors) return;
        if(!_isAlive) return;
        switch (other.name)
        {
            case "SafePointCollider":
                //collided with the safe point
                Debug.Log("Creature saved!");
                _isAlive = false;
                GameMaster.Instance.CreatureSaved();
                Destroy(gameObject);
                break;
            case "DetectCollider":
                //safe point detected
                _detectedSafePoint = true;
                _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, other.transform));
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "KamikazeEnemy")
        {
            _areMonstersClose = false;
            EnemyKamikazeCharacter enemyKamikazeCharacter = other.GetComponent<EnemyKamikazeCharacter>();
            enemyKamikazeCharacter?.CreatureLost(gameObject);
        }
    }
}