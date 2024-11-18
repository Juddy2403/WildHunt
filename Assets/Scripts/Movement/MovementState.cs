
namespace Movement
{
    public abstract class MovementState 
    {
        protected NavMeshMovementBehaviour _movementBehaviour;

        protected MovementState(NavMeshMovementBehaviour movementBehaviour)
        {
            _movementBehaviour = movementBehaviour;
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update();
    }
}