using UnityEngine;

namespace Assets.Scripts.Player.Movement
{
    public sealed class MovementMotor
    {
        public Vector3 FullVelocity => _velocity + _movementVelocity;
        public Vector3 FullHorizontalVelocity => new(FullVelocity.x, 0, FullVelocity.z);

        public Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }
        public Vector3 HorizontalVelocity
        {
            get => new(Velocity.x, 0, Velocity.z);
            set => Velocity = new(value.x, Velocity.y, value.z);
        }

        public Vector3 Movement
        {
            get => _movementVelocity;
            set => _movementVelocity = value;
        }
        public Vector3 HorizontalMovement
        {
            get => new(Movement.x, 0, Movement.z);
            set => Movement = new(value.x, Movement.y, value.z);
        }

        public Vector3 Forward => _rigidbody.transform.forward;
        public Vector3 Right => _rigidbody.transform.right;

        private Vector3 _velocity;
        private Vector3 _movementVelocity;

        private readonly Rigidbody _rigidbody;

        public MovementMotor(Rigidbody rigidbody) => _rigidbody = rigidbody;

        public void ReadFromRigidbody() => _velocity = _rigidbody.linearVelocity - _movementVelocity;
        public void ApplyToRigidbody() => _rigidbody.linearVelocity = FullVelocity;

        public void Accelerate(Vector3 targetVelocity, float acceleration, float dt) =>
            Velocity = Vector3.MoveTowards(Velocity, targetVelocity, acceleration * dt);

        public void AccelerateHorizontal(Vector3 targetVelocity, float acceleration, float dt) =>
            HorizontalVelocity = Vector3.MoveTowards(HorizontalVelocity, targetVelocity, acceleration * dt);

        public void AccelerateHorizontalMovement(Vector3 targetVelocity, float acceleration, float dt) =>
            HorizontalMovement = Vector3.MoveTowards(HorizontalMovement, targetVelocity, acceleration * dt);

        public void AddImpulse(Vector3 impulse) => _velocity += impulse / _rigidbody.mass;
    }
}
