using UnityEngine;

namespace Movement
{
    public abstract class MovementState 
    {
        protected NavMeshMovementBehaviour _movementBehaviour;

        public MovementState(NavMeshMovementBehaviour movementBehaviour)
        {
            _movementBehaviour = movementBehaviour;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}