using Assets.Scripts.Interactables.Core;
using Assets.Scripts.Player.Input.InputContext;
using System;
using UnityEngine;

namespace Assets.Scripts.Player.Interaction
{
    public readonly struct InteractionTarget
    {
        public IInteractable Interactable { get; }
        public Collider Collider { get; }
        public Vector3 Point { get; }
        public Vector3 Normal { get; }

        public InteractionTarget(IInteractable interactable, Collider collider, Vector3 point, Vector3 normal)
        {
            Interactable = interactable;
            Collider = collider;
            Point = point;
            Normal = normal;
        }
    }
    public sealed class PlayerInteractionController : MonoBehaviour
    {
        public event Action<InteractionTarget> HoverStarted;
        public event Action<InteractionTarget, InteractionTarget> HoverChanged;
        public event Action<InteractionTarget> HoverEnded;

        public event Action<InteractionTarget> InteractionStarted;
        public event Action<InteractionTarget> InteractionEnded;

        public InteractionTarget? Hovered => _hovered;
        public InteractionTarget? Active => _active;

        [Header("Settings")]
        [SerializeField] private LayerMask _interactionLayer = ~0;
        [SerializeField, Min(0)] private float _maxInteractionDistance = 1;
        [SerializeField, Min(0)] private float _interactionRadius = 1;

        [Space, Header("References")]
        [SerializeField] private Transform _sender;

        private IInteractionInput _input;

        private InteractionTarget? _hovered;
        private InteractionTarget? _active;

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
            if (_active.HasValue || !_hovered.HasValue) return;

            _active = _hovered;
            _active?.Interactable.BeginInteraction();

            InteractionStarted?.Invoke(_active.Value);
        }
        private void UpdateInteraction(float deltaTime)
        {
            if (!_active.HasValue) return;
            _active.Value.Interactable.UpdateInteraction(deltaTime);
        }

        private void EndInteraction()
        {
            if (!_active.HasValue) return;

            var interactable = _active;
            _active = null;

            interactable.Value.Interactable.EndInteraction();

            InteractionEnded?.Invoke(interactable.Value);
        }

        private void UpdateHover()
        {
            if (TryGetInteractable(out var target))
            {
                if (_hovered.HasValue && _hovered.Value.Collider.gameObject == target.Collider.gameObject) return;

                if (!_hovered.HasValue) HoverStarted?.Invoke(target);
                else
                {
                    EndInteraction();
                    HoverChanged?.Invoke(_hovered.Value, target);
                }

                _hovered = target;
                return;
            }

            if (!_hovered.HasValue) return;

            var previous = _hovered.Value;
            _hovered = null;

            EndInteraction();
            HoverEnded?.Invoke(previous);
        }

        private bool TryGetInteractable(out InteractionTarget target)
        {
            Ray ray = new(_sender.position, _sender.forward);

            if (Physics.SphereCast(ray, _interactionRadius, out var hit, _maxInteractionDistance, _interactionLayer))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    target = new(interactable, hit.collider, hit.point, hit.normal);
                    return true;
                }
            }

            target = default;
            return false;
        }
    }
}
