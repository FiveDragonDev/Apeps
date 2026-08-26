using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Inputs.SOs
{
    [CreateAssetMenu(menuName = "SO/Game/Camera Input Reader")]
    public sealed class CameraInputReader : InputReader
    {
        public event Action<Vector2> LookPerformed;

        [field: SerializeField] public InputActionReference LookActionReference { get; private set; }

        public override void Enable()
        {
            if (_enableCount++ != 0) return;

            Subscribe(LookActionReference, null, OnLook, OnLook, nameof(LookActionReference));

            EnableAction(LookActionReference, nameof(LookActionReference));
        }
        public override void Disable()
        {
            if (--_enableCount != 0) return;

            DisableAction(LookActionReference, nameof(LookActionReference));

            Unsubscribe(LookActionReference, null, OnLook, OnLook, nameof(LookActionReference));
        }

        private void OnLook(InputAction.CallbackContext ctx)
        {
            if (ctx.performed) LookPerformed?.Invoke(ctx.ReadValue<Vector2>());
            else if (ctx.canceled) LookPerformed?.Invoke(Vector2.zero);
        }
    }
}
