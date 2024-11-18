using UnityEngine;

namespace Movement
{
    public class FollowState : MovementState
    {
        private readonly Transform _target;

        public FollowState(NavMeshMovementBehaviour movementBehaviour, Transform target) : base(movementBehaviour)
        {
            _target = target;
        }

        public override void Enter()
        {
            _movementBehaviour.SetTarget(_target);
        }

        public override void Exit()
        {
            _movementBehaviour.SetTarget(null);
        }

        public override void Update() { }
    }
}