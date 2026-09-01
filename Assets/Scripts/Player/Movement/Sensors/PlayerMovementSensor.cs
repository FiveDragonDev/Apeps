using UnityEngine;

namespace Assets.Scripts.Player.Movement.Sensors
{
    public interface IMovementSensor
    {
        bool IsOnSurface { get; }
        Vector3 GroundNormal { get; }

        bool IsOnTooSteepSlope(float slopeAngle);
    }
    public sealed class PlayerMovementSensor : MonoBehaviour, IMovementSensor
    {
        public bool IsOnSurface => _groundChecker != null && _groundChecker.IsOnSurface;
        public Vector3 GroundNormal => _groundChecker != null ? _groundChecker.GroundNormal : Vector3.up;

        [Header("References")]
        [SerializeField] private GroundChecker _groundChecker;

        public bool IsOnTooSteepSlope(float slopeAngle) => IsOnSurface && Vector3.Angle(GroundNormal, Vector3.up) > slopeAngle;
    }
}
