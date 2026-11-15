using UnityEngine;

namespace Assets.Scripts.Player.Movement.Sensors
{
    public interface IMovementSensor
    {
        bool IsOnSurface { get; }
        Vector3 GroundNormal { get; }

        Transform Platform { get; }
        Vector3 PlatformVelocity { get; }

        bool CanStand { get; }

        bool IsOnTooSteepSlope(float slopeAngle);
    }
    public sealed class PlayerMovementSensor : MonoBehaviour, IMovementSensor
    {
        public bool IsOnSurface => _groundChecker != null && _groundChecker.HasObstacle;
        public Vector3 GroundNormal => _groundChecker != null ? _groundChecker.ObstacleNormal : Vector3.up;

        public Transform Platform
        {
            get
            {
                var tracker = GetPlatformTracker();
                return tracker != null ? tracker.transform : null;
            }
        }
        public Vector3 PlatformVelocity
        {
            get
            {
                var tracker = GetPlatformTracker();
                return tracker != null ? tracker.LinearVelocity : Vector3.zero;
            }
        }

        public bool CanStand => _headChecker != null && !_headChecker.HasObstacle;

        [Header("References")]
        [SerializeField] private ObstacleChecker _groundChecker;
        [SerializeField] private ObstacleChecker _headChecker;

        public bool IsOnTooSteepSlope(float slopeAngle) => IsOnSurface && Vector3.Angle(GroundNormal, Vector3.up) > slopeAngle;

        private VelocityTracker GetPlatformTracker()
        {
            var collider = _groundChecker != null ? _groundChecker.HitCollider : null;
            return collider != null ? collider.GetComponentInParent<VelocityTracker>() : null;
        }
    }
}
