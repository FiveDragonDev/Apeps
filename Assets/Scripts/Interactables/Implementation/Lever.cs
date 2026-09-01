using Assets.Scripts.Interactables.Base;
using UnityEngine;

namespace Assets.Scripts.Interactables.Implementation
{
    public sealed class Lever : Toggle
    {
        [Space]
        [SerializeField] private Transform _lever;
        [SerializeField] private Vector3 _rotationAxis = Vector3.right;
        [SerializeField] private float _minAngle;
        [SerializeField] private float _maxAngle;
        [SerializeField, Min(0)] private float _smoothTime = 0.1f;

        private float _angle;
        private float _angleVelocity;

        private void Start()
        {
            _angle = IsOn ? _maxAngle : _minAngle;
            _lever.localRotation = Quaternion.AngleAxis(_angle, in _rotationAxis);
        }
        private void Update()
        {
            var targetAngle = IsOn ? _maxAngle : _minAngle;
            _angle = Mathf.SmoothDampAngle(_angle, targetAngle, ref _angleVelocity, _smoothTime);
            _lever.localRotation = Quaternion.AngleAxis(_angle, in _rotationAxis);
        }
    }
}
