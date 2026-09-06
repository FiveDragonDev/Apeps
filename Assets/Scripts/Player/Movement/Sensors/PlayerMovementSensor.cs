using UnityEngine;

namespace Assets.Scripts.Player.Movement.Sensors
{
    public interface IMovementSensor
    {
        bool IsOnSurface { get; }
        Vector3 GroundNormal { get; }

        bool CanStand { get; }

        bool IsOnTooSteepSlope(float slopeAngle);
    }
    public sealed class PlayerMovementSensor : MonoBehaviour, IMovementSensor
    {
        public bool IsOnSurface => _groundChecker != null && _groundChecker.HasObstacle;
        public Vector3 GroundNormal => _groundChecker != null ? _groundChecker.ObstacleNormal : Vector3.up;

        public bool CanStand => _headChecker != null && !_headChecker.HasObstacle;

        [Header("References")]
        [SerializeField] private ObstacleChecker _groundChecker;
        [SerializeField] private ObstacleChecker _headChecker;

        public bool IsOnTooSteepSlope(float slopeAngle) => IsOnSurface && Vector3.Angle(GroundNormal, Vector3.up) > slopeAngle;
    }
}
