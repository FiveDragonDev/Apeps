using UnityEngine;

namespace Assets.Scripts.Player.Movement.Sensors
{
    public sealed class ObstacleChecker : MonoBehaviour
    {
        public bool HasObstacle { get; private set; }
        public Vector3 ObstacleNormal { get; private set; }
        public Collider HitCollider { get; private set; }

        [Header("Settings")]
        [SerializeField] private LayerMask _obstacleMask = ~0;
        [SerializeField, Min(0)] private float _checkDistance = 1;
        [SerializeField] private Vector3 _direction;
        [SerializeField] private Vector3 _offset;

        [Space, Header("References")]
        [SerializeField] private CapsuleCollider _collider;

        private void FixedUpdate()
        {
            if (_collider == null) return;

            var radius = _collider.radius * 0.99f;
            var height = _collider.height * 0.5f - radius;
            var point1 = transform.position + _offset + _collider.center + Vector3.up * height;
            var point2 = transform.position + _offset + _collider.center - Vector3.up * height;

            var direction = _direction.normalized;
            if (Physics.CapsuleCast(point1, point2, radius, direction, out var hit, _checkDistance, _obstacleMask))
            {
                HasObstacle = true;
                ObstacleNormal = hit.normal;
                HitCollider = hit.collider;
            }
            else
            {
                HasObstacle = false;
                ObstacleNormal = -direction;
                HitCollider = null;
            }
        }
    }
}
