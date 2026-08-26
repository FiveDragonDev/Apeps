using System;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Input.Internal
{
    public sealed class InputValueBinding<T> where T : struct
    {
        private readonly InputAction _action;
        private readonly Action<T> _onChanged;
        private readonly T _cancelValue;

        public InputValueBinding(InputAction action, Action<T> onChanged, T cancelValue = default)
        {
            _action = action;
            _onChanged = onChanged;
            _cancelValue = cancelValue;
        }

        public void Enable()
        {
            if (_action == null) return;

            _action.performed += OnPerformed;
            _action.canceled += OnCanceled;
            _action.Enable();
        }
        public void Disable()
        {
            if (_action == null) return;

            _action.performed -= OnPerformed;
            _action.canceled -= OnCanceled;
            _action.Disable();
        }

        private void OnPerformed(InputAction.CallbackContext ctx) => _onChanged?.Invoke(ctx.ReadValue<T>());
        private void OnCanceled(InputAction.CallbackContext ctx) => _onChanged?.Invoke(_cancelValue);
    }
}