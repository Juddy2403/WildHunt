using UnityEngine;

namespace Movement
{
    public class RunState : MovementState
    {
        private float _timer;
        private const float _movementSpeedMultiplier = 1.5f;
        private Transform _wanderTarget;

        public RunState(NavMeshMovementBehaviour movementBehaviour) : base(movementBehaviour) { }

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

        const float wanderRadius = 10f;
        const float wanderTimer = 1f;
        public override void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= wanderTimer || !_movementBehaviour.Target)
            {
                Vector3 newPos = NavMeshMovementBehaviour.RandomNavSphere(_movementBehaviour.transform.position, wanderRadius);
                _wanderTarget.position = newPos;
                _movementBehaviour.Target = _wanderTarget;
                _timer = 0;
            }
        }
    }
}