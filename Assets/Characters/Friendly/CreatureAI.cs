using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreatureAI : BasicCharacter
{
    private GameObject _playerTarget = null;
    private const float _followRange = 10.0f;
    private const float _idleRange = 4.0f;
    private bool _isTargeted = false;
    private bool _areMonstersClose = false;

    private List<GameObject> _enemiesTargeting = new();

    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();
        if (player) _playerTarget = player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (!_movementBehaviour) return;
        if (_areMonstersClose)
        {
            RunAround();
            return;
        }

        if ((transform.position - _playerTarget.transform.position).sqrMagnitude < _followRange * _followRange &&
            (transform.position - _playerTarget.transform.position).sqrMagnitude > _idleRange * _idleRange &&
            !_isTargeted)
            _movementBehaviour.Target = _playerTarget.transform;
        else _movementBehaviour.Target = null;
        _areMonstersClose = false;
    }

    public float wanderRadius = 20f;
    public float wanderTimer = 1f;

    private float _timer;
    private Transform _wanderTarget;

    private void RunAround()
    {
        if (!_wanderTarget) _wanderTarget = new GameObject("WanderTarget").transform;
        
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
        if (other.name == "KamikazeEnemy")
        {
            _areMonstersClose = true;
            if (!_enemiesTargeting.Contains(other.gameObject))
            {
                _enemiesTargeting.Add(other.gameObject);
                _isTargeted = true;
                EnemyKamikazeCharacter enemyKamikazeCharacter = other.GetComponent<EnemyKamikazeCharacter>();
                if (enemyKamikazeCharacter) enemyKamikazeCharacter.CreatureDetected(gameObject);
            }
        }
    }

    public void OnEnemyStopsTargeting(GameObject enemyObj)
    {
        if (_enemiesTargeting.Contains(enemyObj)) _enemiesTargeting.Remove(enemyObj);
        if (_enemiesTargeting.Count == 0) _isTargeted = false;
    }

    private void OnDestroy()
    {
        foreach (GameObject enemy in _enemiesTargeting)
        {
            if (!enemy) continue;
            EnemyKamikazeCharacter enemyKamikazeCharacter = enemy.GetComponent<EnemyKamikazeCharacter>();
            if (enemyKamikazeCharacter) enemyKamikazeCharacter.TargetDestroyed();
        }
        if(_wanderTarget) Destroy(_wanderTarget.gameObject);
    }
}