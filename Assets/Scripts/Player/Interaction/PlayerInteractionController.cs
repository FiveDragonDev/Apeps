using Assets.Scripts.Interactables;
using Assets.Scripts.Player.Input.InputContext;
using System;
using UnityEngine;

namespace Assets.Scripts.Player.Interaction
{
    public sealed class PlayerInteractionController : MonoBehaviour
    {
        public event Action<IInteractable> HoverStarted;
        public event Action<IInteractable, IInteractable> HoverChanged;
        public event Action HoverEnded;

        public event Action<IInteractable> InteractionStarted;
        public event Action<IInteractable> InteractionEnded;

        public IInteractable Hovered => _hoveredInteractable;
        public IInteractable Active => _activeInteractable;

        [SerializeField] private LayerMask _interactionLayer = ~0;
        [SerializeField, Min(0)] private float _maxInteractionDistance = 1;
        [SerializeField, Min(0)] private float _interactionRadius = 1;

        [SerializeField] private Transform _sender;

        private IInteractionInput _input;

        private Collider _hoveredCollider;
        private IInteractable _hoveredInteractable;
        private IInteractable _activeInteractable;

        private void OnEnable()
        {
            if (_input != null) _input.Interact += OnInteract;
        }
        private void OnDisable()
        {
            if (_input != null) _input.Interact -= OnInteract;

            EndInteraction();
        }
        private void Update()
        {
            UpdateHover();
            UpdateInteraction(Time.deltaTime);
        }

        public void Initialize(IInteractionInput input)
        {
            if (_input != null) _input.Interact -= OnInteract;

            _input = input;

            if (isActiveAndEnabled && _input != null) _input.Interact += OnInteract;
        }

        private void OnInteract(bool started)
        {
            if (started) BeginInteraction();
            else EndInteraction();
        }

        private void BeginInteraction()
        {
            if (_activeInteractable != null || _hoveredInteractable == null) return;

            _activeInteractable = _hoveredInteractable;
            _activeInteractable.BeginInteraction();

            InteractionStarted?.Invoke(_activeInteractable);
        }
        private void UpdateInteraction(float deltaTime) => _activeInteractable?.UpdateInteraction(deltaTime);
        private void EndInteraction()
        {
            if (_activeInteractable == null) return;

            var interactable = _activeInteractable;
            _activeInteractable = null;

            interactable.EndInteraction();

            InteractionEnded?.Invoke(interactable);
        }

        private void UpdateHover()
        {
            if (TryGetInteractable(out var collider, out var interactable))
            {
                if (_hoveredCollider == collider) return;

                var previous = _hoveredInteractable;

                _hoveredCollider = collider;
                _hoveredInteractable = interactable;

                if (previous == null) HoverStarted?.Invoke(interactable);
                else HoverChanged?.Invoke(previous, interactable);

                return;
            }

            if (_hoveredInteractable == null) return;

            _hoveredCollider = null;
            _hoveredInteractable = null;

            HoverEnded?.Invoke();
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
