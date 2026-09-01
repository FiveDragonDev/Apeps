using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public class HoldButton : MonoBehaviour, IInteractable
    {
        public bool IsPressed => _isPressed;

        public UnityEvent<bool> PressedStateChanged;

        private bool _isPressed;

        public virtual void BeginInteraction()
        {
            _isPressed = true;
            PressedStateChanged?.Invoke(_isPressed);
        }
        public virtual void UpdateInteraction(float deltaTime) { }
        public virtual void EndInteraction()
        {
            _isPressed = false;
            PressedStateChanged?.Invoke(_isPressed);
        }
    }
}
