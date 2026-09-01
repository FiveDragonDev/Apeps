using Assets.Scripts.Interactables.Base;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Implementation
{
    public sealed class Jackbox : TimeHoldButton
    {
        public UnityEvent OnOpen;

        [Space]
        [SerializeField] private Transform _top;
        [SerializeField] private Vector3 _rotationAxis = Vector3.right;
        [SerializeField] private float _openAngle;
        [SerializeField, Min(0)] private float _openDelay = 1;
        [SerializeField, Min(0)] private float _smoothTime = 0.05f;

        private float _angle;
        private float _angleVelocity;

        private bool _played;

        private void Update()
        {
            var play = ActiveTime >= _openDelay;
            if (play && !_played)
            {
                OnOpen?.Invoke();
                _played = true;
            }
            var targetAngle = play ? _openAngle : 0;
            _angle = Mathf.SmoothDampAngle(_angle, targetAngle, ref _angleVelocity, _smoothTime);
            _top.localRotation = Quaternion.AngleAxis(_angle, in _rotationAxis);
        }
    }
}
