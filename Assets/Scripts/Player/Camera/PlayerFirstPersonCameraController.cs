using Assets.Scripts.Player.Input.InputContext;
using UnityEngine;

namespace Assets.Scripts.Player.Camera
{
    [DefaultExecutionOrder(100)]
    public sealed class PlayerFirstPersonCameraController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Min(0)] private float _mouseSensitivity = 1;
        [SerializeField, Min(0)] private float _rotationSmoothTime = 0.01f;

        [Space, SerializeField, Range(-90, 0)] private float _minPitch = -90;
        [SerializeField, Range(0, 90)] private float _maxPitch = 90;

        [Space, SerializeField, Range(0, 1)] private float _eyeHeightRatio = 0.9f;
        [SerializeField, Min(0)] private float _heightSmoothTime = 0.1f;

        [Space, Header("References")]
        [SerializeField] private PlayerBody _body;

        private ICameraInput _input;

        private float _heightVelocity;

        private Vector2 _targetRotation;
        private Vector2 _currentRotation;
        private Vector2 _rotationVelocity;

        private void Awake()
        {
            if (_body == null) Debug.LogError("Body transform is not assigned", this);

            var initialYaw = _body != null ? _body.Yaw : 0;
            var initialPitch = transform.localEulerAngles.x;
            if (initialPitch > 180) initialPitch -= 360;

            _currentRotation = new(initialYaw, initialPitch);
            _targetRotation = _currentRotation;
            _rotationVelocity = Vector2.zero;
        }
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void Update()
        {
            if (_input == null) return;

            var lookDelta = _mouseSensitivity * _input.Look;

            _targetRotation.x += lookDelta.x;
            _targetRotation.y = Mathf.Clamp(_targetRotation.y - lookDelta.y, _minPitch, _maxPitch);
        }
        private void LateUpdate()
        {
            if (_body == null) return;

            // height
            float targetHeight = _body.Height * _eyeHeightRatio;
            var position = transform.localPosition;
            position.y = Mathf.SmoothDamp(position.y, targetHeight, ref _heightVelocity, _heightSmoothTime);

            // rotation
            var smoothYaw = Mathf.SmoothDampAngle(_currentRotation.x, _targetRotation.x, ref _rotationVelocity.x, _rotationSmoothTime);
            var smoothPitch = Mathf.SmoothDampAngle(_currentRotation.y, _targetRotation.y, ref _rotationVelocity.y, _rotationSmoothTime);
            _currentRotation = new(smoothYaw, smoothPitch);
            _body.SetYaw(smoothYaw);

            transform.SetLocalPositionAndRotation(position, Quaternion.Euler(smoothPitch, 0, 0));
        }

        public void Initialize(ICameraInput input) => _input = input;
    }
}
