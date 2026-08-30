using Assets.Scripts.Interactables;
using Assets.Scripts.Player.Input.InputContext;
using System;
using UnityEngine;

namespace Assets.Scripts.Player.Interaction
{
    public sealed class PlayerInteractionController : MonoBehaviour
    {
        public event Action<IInteractable> OnHoverStarted;
        public event Action OnHoverEnded;
        public event Action<IInteractable> OnInteracted;

        [SerializeField] private LayerMask _interactionLayer = ~0;
        [SerializeField, Min(0)] private float _maxInteractionDistance = 1;
        [SerializeField, Min(0)] private float _interactionRadius = 1;

        [SerializeField] private Transform _sender;

        private IInteractionInput _input;

        private Collider _currentCollider;
        private Collider _previousCollider;
        private IInteractable _currentInteractable;

        private bool _initialized;

        private void Start() => InitializeInternal();
        private void OnDisable()
        {
            if (_input != null) _input.Interact -= Interact;
        }
        private void Update() => HandleInteraction();

        public void Initialize(IInteractionInput input)
        {
            _input = input;
            InitializeInternal();
        }

        private void InitializeInternal()
        {
            if (_initialized || _input == null) return;
            _input.Interact += Interact;
            _initialized = true;
        }

        private void Interact()
        {
            if (_currentInteractable == null) return;

            _currentInteractable.Interact();
            OnInteracted?.Invoke(_currentInteractable);
        }

        private void HandleInteraction()
        {
            bool found = TryGetInteractable(out var hitCollider, out var interactable);

            if (found)
            {
                if (_currentCollider != hitCollider)
                {
                    _previousCollider = _currentCollider;
                    _currentCollider = hitCollider;
                    _currentInteractable = interactable;

                    OnHoverStarted?.Invoke(interactable);
                }
            }
            else
            {
                if (_currentCollider != null)
                {
                    _currentCollider = null;
                    _currentInteractable = null;
                    OnHoverEnded?.Invoke();
                }
            }

            // _canvasController.ShowInteractionDisplay(found);
        }

        private bool TryGetInteractable(out Collider collider, out IInteractable interactable)
        {
            Ray ray = new(_sender.position, _sender.forward);
            if (Physics.SphereCast(ray, _interactionRadius, out var hit, _maxInteractionDistance, _interactionLayer))
            {
                if (hit.collider.TryGetComponent(out interactable))
                {
                    collider = hit.collider;
                    return true;
                }
            }

            collider = null;
            interactable = null;
            return false;
        }
    }
}
