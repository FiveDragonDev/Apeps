using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public class Toggle : MonoBehaviour, IInteractable
    {
        public UnityEvent<bool> Toggled;

        public bool IsOn => _isOn;

        [SerializeField] private bool _isOnByDefault;

        private bool _isOn;

        private void Awake() => _isOn = _isOnByDefault;

        public void BeginInteraction()
        {
            _isOn = !_isOn;
            Toggled?.Invoke(_isOn);
            OnBeginInteraction();
        }
        public void UpdateInteraction(float deltaTime) => OnUpdateInteraction(deltaTime);
        public void EndInteraction() => OnEndInteraction();

        protected virtual void OnBeginInteraction() { }
        protected virtual void OnUpdateInteraction(float deltaTime) { }
        protected virtual void OnEndInteraction() { }
    }
}
