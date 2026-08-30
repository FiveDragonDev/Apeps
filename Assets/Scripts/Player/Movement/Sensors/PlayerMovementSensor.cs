using UnityEngine;

namespace Assets.Scripts.Player.Movement.Sensors
{
    public interface IMovementSensor
    {
        bool IsOnSurface { get; }
        Vector3 GroundNormal { get; }
    }
    public sealed class PlayerMovementSensor : MonoBehaviour, IMovementSensor
    {
        public bool IsOnSurface => _groundChecker != null && _groundChecker.IsOnSurface;
        public Vector3 GroundNormal => _groundChecker != null ? _groundChecker.GroundNormal : Vector3.up;

        [SerializeField] private GroundChecker _groundChecker;
    }
}
