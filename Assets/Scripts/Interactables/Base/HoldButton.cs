using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public class HoldButton : MonoBehaviour, IInteractable
    {
        public bool IsPressed => _isPressed;

        public UnityEvent<bool> PressedStateChanged;

        private bool _isPressed;

        public void BeginInteraction()
        {
            _isPressed = true;
            PressedStateChanged?.Invoke(_isPressed);
            OnBeginInteraction();
        }
        public void UpdateInteraction(float deltaTime) => OnUpdateInteraction(deltaTime);
        public void EndInteraction()
        {
            _isPressed = false;
            PressedStateChanged?.Invoke(_isPressed);
            OnEndInteraction();
        }

        protected virtual void OnBeginInteraction() { }
        protected virtual void OnUpdateInteraction(float deltaTime) { }
        protected virtual void OnEndInteraction() { }
    }
}
