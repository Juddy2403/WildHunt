using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMovementBehaviour : MovementBehaviour
{
    private NavMeshAgent _navMeshAgent;

    private Vector3 _previousTargetPosition = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _movementSpeed;

        _previousTargetPosition = transform.position;
    }

    const float MOVEMENT_EPSILON = .25f;
    protected override void HandleMovement()
    {
        if (!_target)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        //should the target move we should recalculate our path
        if ((_target.position - _previousTargetPosition).sqrMagnitude > MOVEMENT_EPSILON)
        {
            _navMeshAgent.SetDestination(_target.position);
            _navMeshAgent.isStopped = false;
            _previousTargetPosition = _target.position;
        }

    }
}
