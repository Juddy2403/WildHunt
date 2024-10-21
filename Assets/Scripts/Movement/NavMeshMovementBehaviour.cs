using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMovementBehaviour : MovementBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private MovementState _currentState;
        private Vector3 _previousTargetPosition = Vector3.zero;

        protected override void Awake()
        {
            base.Awake();

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _movementSpeed;
            _previousTargetPosition = transform.position;
            SetState(new IdleState(this));
        }
        protected override void HandleMovement()
        {
            _currentState?.Update();
            
            if (!_target)
            {
                _navMeshAgent.isStopped = true;
                return;
            }

            const float MOVEMENT_EPSILON = .25f;
            //should the target move we should recalculate our path
            if ((_target.position - _previousTargetPosition).sqrMagnitude > MOVEMENT_EPSILON)
            {
                _navMeshAgent.SetDestination(_target.position);
                _navMeshAgent.isStopped = false;
                _previousTargetPosition = _target.position;
            }
        }
        public void SetState(MovementState newState)
        {
            if(_currentState?.GetType() == newState?.GetType()) return;
            //Debug.Log(gameObject.name + " changing state to " + newState.GetType().Name);
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }
        public static Vector3 RandomNavSphere(Vector3 origin, float dist)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;
            randDirection.y = 0;
            randDirection += origin;
            return randDirection;
        }

        private void OnDestroy()
        {
            _currentState?.Exit();
        }
    }
}
