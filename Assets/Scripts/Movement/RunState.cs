using UnityEngine;

namespace Movement
{
    public class RunState : MovementState
    {
        private float _timer;
        private  float _startMovementSpeed;
        private const float _movementSpeed = 10;

        public RunState(NavMeshMovementBehaviour movementBehaviour) : base(movementBehaviour) { }

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

        const float wanderRadius = 10f;
        const float wanderTimer = 1f;
        public override void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= wanderTimer)
            {
                Vector3 newPos = NavMeshMovementBehaviour.RandomNavmeshLocation(_movementBehaviour.transform.position, wanderRadius);
                _movementBehaviour.SetTarget(newPos);
                _timer = 0;
            }
        }
    }
}