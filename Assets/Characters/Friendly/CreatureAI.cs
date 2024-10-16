using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAI : BasicCharacter
{
    private GameObject _playerTarget = null;
    private float _followRange = 10.0f;

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
        if ((transform.position - _playerTarget.transform.position).sqrMagnitude < _followRange * _followRange)
            _movementBehaviour.Target = _playerTarget;
        else
            _movementBehaviour.Target = null;
    }
}
