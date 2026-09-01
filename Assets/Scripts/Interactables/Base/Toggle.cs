using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public class Toggle : MonoBehaviour, IInteractable
    {
        public UnityEvent<bool> OnToggled;

        public bool IsOn => _isOn;

        [SerializeField] private bool _isOnByDefault;

        private bool _isOn;

        private void Awake() => _isOn = _isOnByDefault;

        public virtual void BeginInteraction()
        {
            _isOn = !_isOn;
            OnToggled?.Invoke(_isOn);
        }
        public virtual void UpdateInteraction(float deltaTime) { }
        public virtual void EndInteraction() { }
    }
}
