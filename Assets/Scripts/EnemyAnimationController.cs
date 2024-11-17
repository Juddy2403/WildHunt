using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyAnimationController : MonoBehaviour
{
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Running = Animator.StringToHash("IsRunning");
    private static readonly int Moving = Animator.StringToHash("IsMoving");
    private static readonly int Hit = Animator.StringToHash("GetHit");
    private Vector3 _previousPosition;
    private Animator _animator = null;
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            if (_isMoving != value)
            {
                _isMoving = value;
                _animator.SetBool(Moving, _isMoving);
            }
        }
    }
    private bool _isRunning = false;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            if (_isRunning != value)
            {
                _isRunning = value;
                _animator.SetBool(Running, _isRunning);
            }
        }
    }
    void Awake()
    {
        _previousPosition = transform.root.position;
        _animator = transform.GetComponent<Animator>();
    }
    public void Attack()
    {
        _animator.SetTrigger(Attack1);
    }
    public void GetHit()
    {
        _animator.SetTrigger(Hit);
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovementAnimation();
    }

    private const float EPSILON = 0.0001f;
    void HandleMovementAnimation()
    {
        if (!_animator) return;

        _animator.SetBool(Moving, (transform.root.position - _previousPosition).sqrMagnitude > EPSILON);
        _previousPosition = transform.root.position;
    }
}

