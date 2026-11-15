using Assets.Scripts.Interactables.Base;
using UnityEngine;

namespace Assets.Scripts.Interactables.Implementation
{
    public sealed class Door : Toggle
    {
        [SerializeField] private Transform _door;
        [SerializeField] private Vector3 _axis;
        [SerializeField] private float _closedRotation;
        [SerializeField] private float _openedRotation;
        [SerializeField, Min(0)] private float _rotationSmoothTime = 0.1f;
        [SerializeField, Min(0)] private float _maxSpeed = 0;

        private float _angularVelocity;

        private void Start()
        {
            _door.localRotation = Quaternion.AngleAxis(IsOn ? _openedRotation : _closedRotation, _axis);
        }
        private void Update()
        {
            float targetAngle = IsOn ? _openedRotation : _closedRotation;
            float currentAngle = GetCurrentAngle();

            if (Mathf.Abs(currentAngle - targetAngle) < 0.01f)
            {
                _door.localRotation = Quaternion.AngleAxis(targetAngle, _axis);
                return;
            }

            float newAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref _angularVelocity, _rotationSmoothTime, _maxSpeed > 0 ? _maxSpeed : Mathf.Infinity);
            _door.localRotation = Quaternion.AngleAxis(newAngle, _axis);
        }

        private float GetCurrentAngle()
        {
            Vector3 euler = _door.localRotation.eulerAngles;

            float angle;
            if (_axis == Vector3.right) angle = euler.x;
            else if (_axis == Vector3.up) angle = euler.y;
            else if (_axis == Vector3.forward) angle = euler.z;
            else
            {
                Quaternion closedRot = Quaternion.AngleAxis(_closedRotation, _axis);
                float angleDiff = Quaternion.Angle(closedRot, _door.localRotation);
                angle = _closedRotation + angleDiff * (IsOn ? 1 : -1);
            }
            return angle;
        }
    }
}
