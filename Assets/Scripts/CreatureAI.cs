using System;
using System.Collections.Generic;
using Movement;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureAI : BasicCharacter
{
    [SerializeField] private float _followRange = 15.0f;
    private bool _isAlive = true;
    private bool _detectedSafePoint = false;
    private bool _areMonstersClose = false;
    private NavMeshMovementBehaviour _navMovementBehaviour;

    private void Start()
    {
        _navMovementBehaviour = GetComponent<NavMeshMovementBehaviour>();
        _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
    }

    private void OnEnable()
    {
        if (GameMaster.Instance.IsIndoors) gameObject.GetComponent<Health>().StartHealth = 1;
    }

    private void FixedUpdate()
    {
        //if the player is null, we are indoors or the creature has already found a safe point, return
        if (!_movementBehaviour || !GameMaster.Player || GameMaster.Instance.IsIndoors || _detectedSafePoint) return;
        if (_areMonstersClose) HandleMonstersClose();
        else if (IsPlayerInFollowRange()) HandlePlayerInFollowRange();
        else HandleIdleState();

        //resetting the monsters close flag so we can check it again next frame in OnTriggerStay
        _areMonstersClose = false;
    }

    private bool IsPlayerInFollowRange()
    {
        return (transform.position - GameMaster.Player.transform.position).sqrMagnitude <
               _followRange * _followRange;
    }
    
    private void HandleMonstersClose()
    {
        _navMovementBehaviour.SetState(new RunState(_navMovementBehaviour));
    }

    private void HandlePlayerInFollowRange()
    {
        _navMovementBehaviour.SetNavStopDistance(4);
        _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, GameMaster.Player.transform));
    }

    private void HandleIdleState()
    {
        _navMovementBehaviour.SetState(new IdleState(_navMovementBehaviour));
    }

    private void OnDestroy()
    {
        //on trigger exit is not called when obj destroyed, so i do it here
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius);
        foreach (var collider in colliders) OnTriggerExit(collider);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        _areMonstersClose = true;
        EnemyKamikazeCharacter enemyKamikazeCharacter = other.GetComponent<EnemyKamikazeCharacter>();
        if (enemyKamikazeCharacter) enemyKamikazeCharacter.CreatureDetected(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameMaster.Instance.IsIndoors) return;
        if (!_isAlive) return;
        switch (other.name)
        {
            case "SafePointCollider":
                //collided with the safe point
                Debug.Log("Creature saved!");
                _isAlive = false;
                GameMaster.Instance.CreatureManager.CreatureSaved();
                Destroy(gameObject);
                break;
            case "DetectCollider":
                //safe point detected
                _detectedSafePoint = true;
                _navMovementBehaviour.SetNavStopDistance(0);
                _navMovementBehaviour.SetState(new FollowState(_navMovementBehaviour, other.transform));
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        _areMonstersClose = false;
        var enemyKamikazeCharacter = other.GetComponent<EnemyKamikazeCharacter>();
        enemyKamikazeCharacter?.CreatureLost(gameObject);
    }
}