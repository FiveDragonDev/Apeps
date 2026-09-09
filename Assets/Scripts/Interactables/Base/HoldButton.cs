using Assets.Scripts.Interactables.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public class HoldButton : MonoBehaviour, IInteractable
    {
        public event Action<bool> PressedStateChanged;
        public event Action Enabled;
        public event Action Disabled;

        public bool IsPressed { get; private set; }

        [SerializeField] private UnityEvent<bool> _pressedStateChanged;
        [SerializeField] private UnityEvent _enabled;
        [SerializeField] private UnityEvent _disabled;

        public void BeginInteraction()
        {
            IsPressed = true;
            RaisePressedStateChanged(true);
            RaiseEnabled();
            OnBeginInteraction();
        }
        public void UpdateInteraction(float deltaTime) => OnUpdateInteraction(deltaTime);
        public void EndInteraction()
        {
            IsPressed = false;
            RaisePressedStateChanged(false);
            RaiseDisabled();
            OnEndInteraction();
        }

        protected virtual void OnBeginInteraction() { }
        protected virtual void OnUpdateInteraction(float deltaTime) { }
        protected virtual void OnEndInteraction() { }

        private void RaisePressedStateChanged(bool isPressed)
        {
            _pressedStateChanged?.Invoke(isPressed);
            PressedStateChanged?.Invoke(isPressed);
        }
        private void RaiseEnabled()
        {
            _enabled?.Invoke();
            Enabled?.Invoke();
        }
        private void RaiseDisabled()
        {
            _disabled?.Invoke();
            Disabled?.Invoke();
        }
    }
}
