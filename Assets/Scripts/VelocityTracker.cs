using UnityEngine;

namespace Assets.Scripts
{
    [DefaultExecutionOrder(-1000)]
    public sealed class VelocityTracker : MonoBehaviour
    {
        public Vector3 LinearVelocity => _linearVelocity;
        public Vector3 AngularVelocity => _angularVelocity;

        private Rigidbody _rigidbody;

        private Vector3 _linearVelocity;
        private Vector3 _angularVelocity;

        private Vector3 _lastPosition;
        private Quaternion _lastRotation;

        private void Awake() => TryGetComponent(out _rigidbody);
        private void Start()
        {
            _lastPosition = GetPosition();
            _lastRotation = GetRotation();
        }
        private void FixedUpdate()
        {
            var currentPosition = GetPosition();
            var currentRotation = GetRotation();

            _linearVelocity = (currentPosition - _lastPosition) / Time.fixedDeltaTime;
            _angularVelocity = GetAngularVelocity(currentRotation);
            
            _lastPosition = currentPosition;
            _lastRotation = currentRotation;
        }

        public Vector3 GetAngularVelocity(Quaternion currentRotation)
        {
            float dt = Time.fixedDeltaTime;
            if (dt <= 0) return Vector3.zero;

            var delta = currentRotation * Quaternion.Inverse(_lastRotation);

            if (delta.w < 0) delta = new(-delta.x, -delta.y, -delta.z, -delta.w);

            delta.ToAngleAxis(out var angleDeg, out var axis);
            if (angleDeg > 180) angleDeg -= 360;

            return axis * (angleDeg * Mathf.Deg2Rad) / dt;
        }

        private Vector3 GetPosition() => _rigidbody != null ? _rigidbody.position : transform.position;
        private Quaternion GetRotation() => _rigidbody != null ? _rigidbody.rotation : transform.rotation;
    }
}
