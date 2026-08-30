using Assets.Scripts.Player.Input.InputContext;
using Assets.Scripts.Player.Movement.Sensors;

namespace Assets.Scripts.Player.Movement
{
    public sealed class MovementContext
    {
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
