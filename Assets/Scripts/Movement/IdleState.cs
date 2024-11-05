using UnityEngine;

namespace Movement
{
    public class IdleState : MovementState
    {
        private float _idleTimer = 0f;
        private  float _startMovementSpeed = 0.1f;
        private const float _movementSpeed = 2;
        private Transform _wanderTarget;

        public IdleState(NavMeshMovementBehaviour movementBehaviour) : base(movementBehaviour) { }

        public override void Enter()
        {
            _movementBehaviour.SetNavStopDistance(0);
            _wanderTarget = new GameObject("WanderTarget").transform;
            _startMovementSpeed = _movementBehaviour.MovementSpeed;
            _movementBehaviour.MovementSpeed = _movementSpeed;
        }

        public override void Exit()
        {
            if (_wanderTarget) Object.Destroy(_wanderTarget.gameObject);
            _movementBehaviour.MovementSpeed = _startMovementSpeed;
            _movementBehaviour.Target = null;
        }
        private const float _wanderRadius = 10f;
        private const float _idleWaitTime = 4f;
        public override void Update()
        {
            _idleTimer += Time.deltaTime;
            if (_idleTimer >= _idleWaitTime)
            {
                Vector3 newPos = NavMeshMovementBehaviour.RandomNavmeshLocation(_movementBehaviour.transform.position, Random.Range(3, _wanderRadius));
                _wanderTarget.position = newPos;
                _movementBehaviour.Target = _wanderTarget;
                _idleTimer = 0f;
            }
        }
    }
}