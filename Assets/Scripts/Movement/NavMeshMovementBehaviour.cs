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
        public override float MovementSpeed
        {
            get { return _movementSpeed; }
            set
            {
                _movementSpeed = value; 
                _navMeshAgent.speed = _movementSpeed;
            }
        }
        public void SetNavStopDistance(float distance)
        {
            _navMeshAgent.stoppingDistance = distance;
        }
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
            //if the new state is the same, but not of type Follow State (target can be different), return
            if(_currentState?.GetType() == newState?.GetType() && _currentState?.GetType() != typeof(FollowState))
                return;
            //Debug.Log(gameObject.name + " changing state to " + newState?.GetType().Name);
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }
        public static Vector3 RandomNavmeshLocation(Vector3 origin, float radius) {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += origin;
            NavMeshHit hit; 
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
                finalPosition = hit.position;            
            }
            finalPosition.y += 0.5f;
            return finalPosition;
        }

        private void OnDestroy()
        {
            _currentState?.Exit();
        }
    }
}
