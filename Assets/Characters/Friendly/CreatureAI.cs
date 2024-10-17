using System;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAI : BasicCharacter
{
    private GameObject _playerTarget = null;
    private const float _followRange = 10.0f;
    private const float _idleRange = 4.0f;
    private bool _isTargeted = false;

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
        if ((transform.position - _playerTarget.transform.position).sqrMagnitude < _followRange * _followRange &&
            (transform.position - _playerTarget.transform.position).sqrMagnitude > _idleRange * _idleRange &&
            !_isTargeted)
            _movementBehaviour.Target = _playerTarget;
        else _movementBehaviour.Target = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "KamikazeEnemy" && !_enemiesTargeting.Contains(other.gameObject))
        {
            _enemiesTargeting.Add(other.gameObject);
            _isTargeted = true;
            EnemyKamikazeCharacter enemyKamikazeCharacter = other.GetComponent<EnemyKamikazeCharacter>();
            if (enemyKamikazeCharacter) enemyKamikazeCharacter.CreatureDetected(gameObject);
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
    }
}