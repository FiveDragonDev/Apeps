using Assets.Scripts.Interactables.Base;
using UnityEngine;

namespace Assets.Scripts.Interactables.Implementation
{
    public sealed class Button : HoldButton
    {
        [Space]
        [SerializeField] private Transform _button;
        [SerializeField] private Vector3 _depthAxis = Vector3.up;
        [SerializeField, Min(0)] private float _pressDepth = 1;
        [SerializeField, Min(0)] private float _smoothTime = 0.1f;

        private Vector3 _initialPosition;
        private Vector3 _pressedPosition;
        private Vector3 _velocity;

        private void Start()
        {
            _initialPosition = _button.localPosition;
            _pressedPosition = _initialPosition - _depthAxis.normalized * _pressDepth;
        }
        private void Update()
        {
            var targetPosition = IsPressed ? _pressedPosition : _initialPosition;
            _button.localPosition = Vector3.SmoothDamp(_button.localPosition, targetPosition, ref _velocity, _smoothTime);
        }
    }
}
