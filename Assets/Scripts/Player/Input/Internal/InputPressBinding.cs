using System;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Input.Internal
{
    public sealed class InputPressBinding
    {
        private readonly InputAction _action;
        private readonly Action _onPressed;

        public InputPressBinding(InputAction action, Action onPressed)
        {
            _action = action;
            _onPressed = onPressed;
        }

        public void Enable()
        {
            if (_action == null) return;

            _action.started += OnStarted;
            _action.Enable();
        }
        public void Disable()
        {
            if (_action == null) return;

            _action.started -= OnStarted;
            _action.Disable();
        }

        private void OnStarted(InputAction.CallbackContext _) => _onPressed?.Invoke();
    }
}
