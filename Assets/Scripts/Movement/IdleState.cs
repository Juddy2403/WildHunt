using UnityEngine;

namespace Movement
{
    public class IdleState : MovementState
    {
        private float _idleTimer = 0f;
        private  float _startMovementSpeed = 0.1f;
        private const float _movementSpeed = 2;

        public IdleState(NavMeshMovementBehaviour movementBehaviour) : base(movementBehaviour) { }

        public override void Enter()
        {
            _movementBehaviour.SetNavStopDistance(0);
            _startMovementSpeed = _movementBehaviour.MovementSpeed;
            _movementBehaviour.MovementSpeed = _movementSpeed;
        }

        public override void Exit()
        {
            _movementBehaviour.MovementSpeed = _startMovementSpeed;
            _movementBehaviour.SetTarget(null);
        }
        private const float _wanderRadius = 10f;
        private const float _idleWaitTime = 4f;
        public override void Update()
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer >= _idleWaitTime)
            {
                Vector3 newPos = NavMeshMovementBehaviour.RandomNavmeshLocation(_movementBehaviour.transform.position, Random.Range(3, _wanderRadius));
                _movementBehaviour.SetTarget(newPos);
                _idleTimer = 0f;
            }
        }
    }
}