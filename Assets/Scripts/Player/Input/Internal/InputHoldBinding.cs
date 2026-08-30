using System;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Input.Internal
{
    public sealed class InputHoldBinding
    {
        private readonly InputAction _action;
        private readonly Action<bool> _onChanged;

        public InputHoldBinding(InputAction action, Action<bool> onChanged)
        {
            _action = action;
            _onChanged = onChanged;
        }

        public void Enable()
        {
            if (_action == null) return;

            _action.started += OnStarted;
            _action.canceled += OnCanceled;
            _action.Enable();
        }
        public void Disable()
        {
            if (_action == null) return;

            _action.started -= OnStarted;
            _action.canceled -= OnCanceled;
            _action.Disable();
        }

        private void OnStarted(InputAction.CallbackContext _) => _onChanged?.Invoke(true);
        private void OnCanceled(InputAction.CallbackContext _) => _onChanged?.Invoke(false);
    }
}