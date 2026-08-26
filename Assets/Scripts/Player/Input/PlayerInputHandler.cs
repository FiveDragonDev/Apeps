using Assets.Scripts.Player.Input.Actions;
using Assets.Scripts.Player.Input.Context;
using Assets.Scripts.Player.Input.Internal;
using UnityEngine;

namespace Assets.Scripts.Player.Input
{
    public sealed class PlayerInputHandler : MonoBehaviour
    {
        public IMovementContext Movement => _movement;
        public ICameraContext Camera => _camera;

        [Header("Actions")]
        [SerializeField] private MovementActions _movementActions;
        [SerializeField] private CameraActions _cameraActions;

        private readonly MovementContext _movement = new();
        private readonly CameraContext _camera = new();

        private InputValueBinding<Vector2> _move;
        private InputButtonBinding _sprint;
        private InputPressBinding _jump;
        private InputValueBinding<Vector2> _look;

        private void Awake()
        {
            _move = new(_movementActions.Move, i => _movement.Move = i, Vector2.zero);
            _sprint = new(_movementActions.Sprint, i => _movement.Sprint = i);
            _jump = new(_movementActions.Jump, _movement.RaiseJump);

            _look = new(_cameraActions.Look, i => _camera.Look = i, Vector2.zero);
        }
        private void OnEnable()
        {
            _move?.Enable();
            _sprint?.Enable();
            _jump?.Enable();
            _look?.Enable();
        }
        private void OnDisable()
        {
            _move?.Disable();
            _sprint?.Disable();
            _jump?.Disable();
            _look?.Disable();
        }
    }
}
