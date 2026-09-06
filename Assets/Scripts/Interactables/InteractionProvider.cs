using UnityEngine;

namespace Assets.Scripts.Interactables
{
    public sealed class InteractionProvider : MonoBehaviour, IInteractable
    {
        [SerializeField] private MonoBehaviour _coreInteractable;

        private IInteractable _interactable;

        private void Awake()
        {
            _interactable = _coreInteractable as IInteractable;
            if (_interactable == null)
            {
                Debug.LogError($"No valid IInteractable found on {name}", this);
                return;
            }
        }

        public void BeginInteraction() => _interactable.BeginInteraction();
        public void UpdateInteraction(float deltaTime) => _interactable.UpdateInteraction(deltaTime);
        public void EndInteraction() => _interactable.EndInteraction();
    }
}
