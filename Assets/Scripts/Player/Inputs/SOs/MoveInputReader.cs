using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Inputs.SOs
{
    [CreateAssetMenu(menuName = "SO/Game/Move Input Reader")]
    public sealed class MoveInputReader : InputReader
    {
        public event Action<Vector2> MovePerformed;
        public event Action<bool> SprintChanged;
        public event Action JumpPressed;

        [field: SerializeField] public InputActionReference MoveActionReference { get; private set; }
        [field: SerializeField] public InputActionReference SprintActionReference { get; private set; }
        [field: SerializeField] public InputActionReference JumpActionReference { get; private set; }

        public override void Enable()
        {
            if (_enableCount++ != 0) return;

            Subscribe(MoveActionReference, null, OnMove, OnMove, nameof(MoveActionReference));
            Subscribe(SprintActionReference, OnSprint, null, OnSprint, nameof(SprintActionReference));
            Subscribe(JumpActionReference, OnJump, null, null, nameof(JumpActionReference));

            EnableAction(MoveActionReference, nameof(MoveActionReference));
            EnableAction(SprintActionReference, nameof(SprintActionReference));
            EnableAction(JumpActionReference, nameof(JumpActionReference));
        }

        public override void Disable()
        {
            if (_enableCount < 0) return;
            if (--_enableCount != 0) return;

            DisableAction(MoveActionReference, nameof(MoveActionReference));
            DisableAction(SprintActionReference, nameof(SprintActionReference));
            DisableAction(JumpActionReference, nameof(JumpActionReference));

            Unsubscribe(MoveActionReference, null, OnMove, OnMove, nameof(MoveActionReference));
            Unsubscribe(SprintActionReference, OnSprint, null, OnSprint, nameof(SprintActionReference));
            Unsubscribe(JumpActionReference, OnJump, null, null, nameof(JumpActionReference));
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) MovePerformed?.Invoke(ctx.ReadValue<Vector2>());
            else if (ctx.canceled) MovePerformed?.Invoke(Vector2.zero);
        }
        private void OnSprint(InputAction.CallbackContext ctx)
        {
            if (ctx.started) SprintChanged?.Invoke(true);
            else if (ctx.canceled) SprintChanged?.Invoke(false);
        }
        private void OnJump(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) JumpPressed?.Invoke();
        }
    }
}
