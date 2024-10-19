using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatureAI : BasicCharacter
{
    private GameObject _playerTarget = null;
    private const float _followRange = 10.0f;
    private const float _idleRange = 4.0f;
    private bool _areMonstersClose = false;

    private List<GameObject> _enemiesTargeting = new();

    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        if (player) _playerTarget = player.gameObject;
        if (!_wanderTarget) _wanderTarget = new GameObject("WanderTarget").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (!_movementBehaviour) return;
        if (_areMonstersClose) RunAround();
        else if (IsPlayerInFollowRange() && IsPlayerNotTooClose())
            _movementBehaviour.Target = _playerTarget.transform;
        else _movementBehaviour.Target = null;
        _areMonstersClose = false;
    }

    private bool IsPlayerNotTooClose()
    {
        return (transform.position - _playerTarget.transform.position).sqrMagnitude > _idleRange * _idleRange;
    }

    private bool IsPlayerInFollowRange()
    {
        return (transform.position - _playerTarget.transform.position).sqrMagnitude < _followRange * _followRange;
    }

    private float _timer;
    private Transform _wanderTarget;

    private void RunAround()
    {
        const float wanderRadius = 5f;
        const float wanderTimer = 1f;
        _timer += Time.deltaTime;
        if (_timer >= wanderTimer || !_movementBehaviour.Target)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            _wanderTarget.position = newPos;
            _movementBehaviour.Target = _wanderTarget;
            _timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection.y = 0;
        randDirection += origin;
        return randDirection;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name == "KamikazeEnemy") _areMonstersClose = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "KamikazeEnemy")
        {
            EnemyKamikazeCharacter enemyKamikazeCharacter = other.GetComponent<EnemyKamikazeCharacter>();
            if (enemyKamikazeCharacter) enemyKamikazeCharacter.CreatureDetected(gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "KamikazeEnemy") _areMonstersClose = false;
    }

    private void OnDestroy()
    {
        if (_wanderTarget) Destroy(_wanderTarget.gameObject);
    }
}