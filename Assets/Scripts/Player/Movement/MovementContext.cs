using Assets.Scripts.Player.Input.InputContext;
using Assets.Scripts.Player.Movement.Sensors;
using UnityEngine;

namespace Assets.Scripts.Player.Movement
{
    public sealed class MovementContext
    {
        public bool IsGrounded => Sensor.IsOnSurface && !Sensor.IsOnTooSteepSlope(Config.SlopeAngle);
        public bool IsSprinting => Input.Sprint;
        public Vector3 HorizontalVelocity => Motor.HorizontalVelocity;
        public float CurrentSpeed => HorizontalVelocity.magnitude;
        public bool IsMoving => HorizontalVelocity.sqrMagnitude > 0.01f;
        public float MaxSpeed => Mathf.Max(Config.WalkSpeed, Config.SprintSpeed);

        public IMovementInput Input { get; }
        public MovementMotor Motor { get; }
        public MovementConfig Config { get; }
        public IMovementSensor Sensor { get; }

        public MovementContext(IMovementInput input, MovementMotor motor, MovementConfig config, IMovementSensor sensor)
        {
            Input = input;
            Motor = motor;
            Config = config;
            Sensor = sensor;
        }
    }
}
