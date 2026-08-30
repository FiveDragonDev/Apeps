using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables
{
    public sealed class Lever : MonoBehaviour, IInteractable
    {
        public UnityEvent<bool> Switched;

        [SerializeField] private bool _initialActive;

        [Space]
        [SerializeField] private Transform _lever;
        [SerializeField] private Vector3 _rotationAxis = Vector3.right;
        [SerializeField] private float _minAngle;
        [SerializeField] private float _maxAngle;
        [SerializeField, Min(0)] private float _smoothTime = 0.1f;

        private float _angle;
        private float _angleVelocity;

        private bool _active;

        private void Start()
        {
            _active = _initialActive;

            _angle = _active ? _maxAngle : _minAngle;
            _lever.localRotation = Quaternion.AngleAxis(_angle, in _rotationAxis);
        }
        private void Update()
        {
            var targetAngle = _active ? _maxAngle : _minAngle;
            _angle = Mathf.SmoothDampAngle(_angle, targetAngle, ref _angleVelocity, _smoothTime);
            _lever.localRotation = Quaternion.AngleAxis(_angle, in _rotationAxis);
        }

        public void Interact()
        {
            _active = !_active;
            Switched?.Invoke(_active);
        }
    }
}
