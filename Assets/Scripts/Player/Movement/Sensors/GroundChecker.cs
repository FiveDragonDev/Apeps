using UnityEngine;

namespace Assets.Scripts.Player.Movement.Sensors
{
    public sealed class GroundChecker : MonoBehaviour
    {
        public bool IsOnSurface { get; private set; }
        public Vector3 GroundNormal { get; private set; }

        [SerializeField] private LayerMask _groundMask = ~0;
        [SerializeField, Min(0)] private float _checkDistance = 1;

        [SerializeField] private CapsuleCollider _collider;

        private void FixedUpdate()
        {
            if (_collider == null) return;

            var radius = _collider.radius * 0.99f;
            var height = _collider.height * 0.5f - radius;
            var point1 = transform.position + _collider.center + Vector3.up * height;
            var point2 = transform.position + _collider.center - Vector3.up * height;

            if (Physics.CapsuleCast(point1, point2, radius, Vector3.down, out var hit, _checkDistance, _groundMask))
            {
                IsOnSurface = true;
                GroundNormal = hit.normal;
            }
            else
            {
                IsOnSurface = false;
                GroundNormal = Vector3.up;
            }
        }
    }
}
