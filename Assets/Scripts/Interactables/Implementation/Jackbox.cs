using Assets.Scripts.Interactables.Base;
using UnityEngine;

namespace Assets.Scripts.Interactables.Implementation
{
    public sealed class Jackbox : ProgressHoldButton
    {
        [Space]
        [SerializeField] private Transform _top;
        [SerializeField] private Vector3 _rotationAxis = Vector3.right;
        [SerializeField] private float _openAngle;
        [SerializeField, Min(0)] private float _smoothTime = 0.05f;

        private float _angle;
        private float _angleVelocity;

        private void Update()
        {
            var targetAngle = IsComplete ? _openAngle : 0;
            _angle = Mathf.SmoothDampAngle(_angle, targetAngle, ref _angleVelocity, _smoothTime);
            _top.localRotation = Quaternion.AngleAxis(_angle, in _rotationAxis);
        }
    }
}
