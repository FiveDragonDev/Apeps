using Assets.Scripts.Player.Input.Actions;
using Assets.Scripts.Player.Input.InputContext;
using Assets.Scripts.Player.Input.Internal;
using UnityEngine;

namespace Assets.Scripts.Player.Input
{
    public sealed class PlayerInputHandler : MonoBehaviour
    {
        public IMovementInput Movement => _movement;
        public ICameraInput Camera => _camera;
        public IInteractionInput Interaction => _interaction;

        [SerializeField] private MovementActions _movementActions = new();
        [Space, SerializeField] private CameraActions _cameraActions = new();
        [Space, SerializeField] private InteractionActions _interactionActions = new();

        private readonly MovementInput _movement = new();
        private readonly CameraInput _camera = new();
        private readonly InteractionInput _interaction = new();

        private InputValueBinding<Vector2> _move;
        private InputHoldBinding _sprint;
        private InputPressBinding _jump;
        private InputValueBinding<Vector2> _look;
        private InputHoldBinding _interact;

        private void Awake()
        {
            _move = new(_movementActions.Move, _movement.SetMove, Vector2.zero);
            _sprint = new(_movementActions.Sprint, _movement.SetSprint);
            _jump = new(_movementActions.Jump, _movement.RaiseJump);

            _look = new(_cameraActions.Look, _camera.SetLook, Vector2.zero);

            _interact = new(_interactionActions.Interact, _interaction.RaiseInteract);
        }
        private void OnEnable()
        {
            _move?.Enable();
            _sprint?.Enable();
            _jump?.Enable();
            _look?.Enable();
            _interact?.Enable();
        }
        private void OnDisable()
        {
            _move?.Disable();
            _sprint?.Disable();
            _jump?.Disable();
            _look?.Disable();
            _interact?.Disable();
        }
    }
}
