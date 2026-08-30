using UnityEngine;

namespace Assets.Scripts.Player.Movement
{
    public sealed class MovementMotor
    {
        public Vector3 Velocity
        {
            get => _rigidbody.linearVelocity;
            set => _rigidbody.linearVelocity = value;
        }

        public Vector3 HorizontalVelocity
        {
            get => new(Velocity.x, 0, Velocity.z);
            set => Velocity = new(value.x, Velocity.y, value.z);
        }

        public Vector3 Forward => _rigidbody.transform.forward;
        public Vector3 Right => _rigidbody.transform.right;

        private readonly Rigidbody _rigidbody;

        public MovementMotor(Rigidbody rigidbody) => _rigidbody = rigidbody;

        public void Accelerate(Vector3 targetVelocity, float acceleration, float dt) =>
            Velocity = Vector3.MoveTowards(Velocity, targetVelocity, acceleration * dt);
        public void AccelerateHorizontal(Vector3 targetVelocity, float acceleration, float dt) =>
            HorizontalVelocity = Vector3.MoveTowards(HorizontalVelocity, targetVelocity, acceleration * dt);

        public void AddImpulse(Vector3 impulse) => _rigidbody.AddForce(impulse, ForceMode.Impulse);
    }
}
