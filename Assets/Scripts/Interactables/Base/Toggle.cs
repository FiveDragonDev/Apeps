using Assets.Scripts.Interactables.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public class Toggle : MonoBehaviour, IInteractable
    {
        public event Action<bool> Toggled;
        public event Action Enabled;
        public event Action Disabled;

        public bool IsOn { get; private set; }

        [SerializeField] private UnityEvent<bool> _toggled;
        [SerializeField] private UnityEvent _enabled;
        [SerializeField] private UnityEvent _disabled;

        [SerializeField] private bool _isOnByDefault;

        private void Awake() => IsOn = _isOnByDefault;

        public void BeginInteraction()
        {
            IsOn = !IsOn;

            RaiseToggled(IsOn);
            if (IsOn) RaiseEnabled();
            else RaiseDisabled();

            OnBeginInteraction();
        }
        public void UpdateInteraction(float deltaTime) => OnUpdateInteraction(deltaTime);
        public void EndInteraction() => OnEndInteraction();

        protected virtual void OnBeginInteraction() { }
        protected virtual void OnUpdateInteraction(float deltaTime) { }
        protected virtual void OnEndInteraction() { }

        private void RaiseToggled(bool toggle)
        {
            Toggled?.Invoke(toggle);
            _toggled?.Invoke(toggle);
        }
        private void RaiseEnabled()
        {
            Enabled?.Invoke();
            _enabled?.Invoke();
        }
        private void RaiseDisabled()
        {
            Disabled?.Invoke();
            _disabled?.Invoke();
        }
    }
}
