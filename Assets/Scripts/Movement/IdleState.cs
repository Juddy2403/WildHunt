using UnityEngine;

namespace Movement
{
    public class IdleState : MovementState
    {
        private float _idleTimer = 0f;
        private const float _movementSpeedMultiplier = 0.4f;
        private Transform _wanderTarget;

        public IdleState(NavMeshMovementBehaviour movementBehaviour) : base(movementBehaviour) { }

        public override void Enter()
        {
            _wanderTarget = new GameObject("WanderTarget").transform;
            _movementBehaviour.MovementSpeed *= _movementSpeedMultiplier;
        }

        public override void Exit()
        {
            if (_wanderTarget) Object.Destroy(_wanderTarget.gameObject);
            _movementBehaviour.MovementSpeed /= _movementSpeedMultiplier;
            _movementBehaviour.Target = null;
        }
        private const float _wanderRadius = 10f;
        private const float _idleWaitTime = 4f;
        public override void Update()
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer >= _idleWaitTime)
            {
                Vector3 newPos = NavMeshMovementBehaviour.RandomNavSphere(_movementBehaviour.transform.position, _wanderRadius);
                _wanderTarget.position = newPos;
                _movementBehaviour.Target = _wanderTarget;
                _idleTimer = 0f;
            }
        }
    }
}