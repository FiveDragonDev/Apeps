using Assets.Scripts.Player.Input.InputContext;
using Assets.Scripts.Player.Movement.Sensors;
using Assets.Scripts.Player.Movement.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Player.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerMovementController : MonoBehaviour
    {
        public MovementContext Context { get; private set; }

        [SerializeField] private MovementConfig _config;

        [Space, SerializeField] private PlayerMovementSensor _sensor;

        private IMovementInput _input;
        private MovementMotor _motor;
        private MovementStateMachine _locomotion;

        private bool _initialized;

        private void Start() => InitializeInternal();
        private void Update() => _locomotion.Update(Time.deltaTime);
        private void FixedUpdate() => _locomotion.FixedUpdate(Time.fixedDeltaTime);

        public void Initialize(IMovementInput movementInput)
        {
            _input = movementInput;
            InitializeInternal();
        }

        private void InitializeInternal()
        {
            if (_initialized || _input == null) return;
            _motor = new(GetComponent<Rigidbody>());

            Context = new(_input, _motor, _config, _sensor);

            _locomotion = new(Context);

            _locomotion.RegisterState<GroundedState>();
            _locomotion.RegisterState<AirborneState>();
            _locomotion.Initialize();

            _locomotion.SetInitialState<GroundedState>();

            _initialized = true;
        }
    }
}
