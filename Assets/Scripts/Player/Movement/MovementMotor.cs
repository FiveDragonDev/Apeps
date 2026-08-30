using UnityEngine;

namespace Assets.Scripts.Player.Movement
{
    public sealed class MovementMotor
    {
        public Vector3 Velocity => _rigidbody.linearVelocity;
        public Vector3 HorizontalVelocity
        {
            get => new(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);
            set => _rigidbody.linearVelocity = new(value.x, _rigidbody.linearVelocity.y, value.z);
        }

        public Vector3 Forward => _rigidbody.transform.forward;
        public Vector3 Right => _rigidbody.transform.right;

        private readonly Rigidbody _rigidbody;

        public MovementMotor(Rigidbody rigidbody) => _rigidbody = rigidbody;

        public void AccelerateHorizontal(Vector3 targetVelocity, float acceleration, float dt) =>
            HorizontalVelocity = Vector3.MoveTowards(HorizontalVelocity, targetVelocity, acceleration * dt);

        public void AddImpulse(Vector3 impulse) => _rigidbody.AddForce(impulse, ForceMode.Impulse);
    }
}
