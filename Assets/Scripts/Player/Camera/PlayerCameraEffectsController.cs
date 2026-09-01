using Assets.Scripts.Player.Input.InputContext;
using Assets.Scripts.Player.Movement;
using UnityEngine;

namespace Assets.Scripts.Player.Camera
{
    [DefaultExecutionOrder(200)]
    public sealed class PlayerCameraEffectsController : MonoBehaviour
    {
        [Space, Header("Head Bob")]
        [SerializeField, Min(0)] private float _walkBobSpeed = 8;
        [SerializeField, Min(0)] private float _walkBobAmountY = 0.05f;
        [SerializeField, Min(0)] private float _walkBobAmountX = 0.03f;
        [SerializeField, Min(0)] private float _sprintBobSpeedMultiplier = 1.5f;
        [SerializeField, Min(0)] private float _sprintBobAmountMultiplier = 1.5f;

        [Space, Header("Tilt")]
        [SerializeField, Min(0)] private float _strafeTiltAngle = 3;
        [SerializeField, Min(0)] private float _turnTiltAngle = 2;
        [SerializeField, Min(0)] private float _tiltSmoothTime = 0.1f;

        [Space, Header("Shake")]
        [SerializeField, Min(0)] private float _shakeAmplitude = 0.02f;
        [SerializeField, Min(0)] private float _shakeFrequency = 15;
        [SerializeField, Min(0)] private float _shakeSprintMultiplier = 2;
        [SerializeField, Min(0)] private float _shakeSmoothTime = 0.05f;

        [Space, Header("References")]
        [SerializeField] private Transform _cameraTransform;

        private MovementContext _movement;
        private ICameraInput _input;

        private Vector3 _baseLocalPosition;

        private float _bobTimer;

        private float _tiltX;
        private float _tiltZ;
        private float _tiltVelocityX;
        private float _tiltVelocityZ;

        private Vector3 _currentShakeOffset;
        private Vector3 _shakeVelocity;

        private void Awake()
        {
            if (_cameraTransform == null) _cameraTransform = transform;
            _baseLocalPosition = _cameraTransform.localPosition;
        }
        private void LateUpdate()
        {
            if (_movement == null || _input == null) return;

            var bob = CalculateHeadBob();
            var tilt = CalculateTilt();
            var shake = CalculateShake();

            _cameraTransform.SetLocalPositionAndRotation(_baseLocalPosition + bob + shake, _cameraTransform.localRotation * tilt);
        }

        public void Initialize(ICameraInput input, MovementContext movement)
        {
            _input = input;
            _movement = movement;
        }

        private Vector3 CalculateHeadBob()
        {
            if (!_movement.IsGrounded || !_movement.IsMoving) return Vector3.zero;

            float speed = _movement.CurrentSpeed;

            float speedRatio = Mathf.Clamp01(speed / Mathf.Max(_movement.Config.SprintSpeed, 0.001f));

            float bobSpeed = _walkBobSpeed;
            float amountX = _walkBobAmountX;
            float amountY = _walkBobAmountY;

            if (_movement.IsSprinting)
            {
                bobSpeed *= _sprintBobSpeedMultiplier;
                amountX *= _sprintBobAmountMultiplier;
                amountY *= _sprintBobAmountMultiplier;
            }

            _bobTimer += Time.deltaTime * bobSpeed * speedRatio;

            return new(Mathf.Cos(_bobTimer) * amountX, Mathf.Sin(_bobTimer * 2f) * amountY, 0f);
        }

        private Quaternion CalculateTilt()
        {
            Vector2 move = _movement.Input.Move;

            float targetTiltZ = -move.x * _strafeTiltAngle;
            float targetTiltX = move.y * _turnTiltAngle;

            _tiltX = Mathf.SmoothDamp(_tiltX, targetTiltX, ref _tiltVelocityX, _tiltSmoothTime);
            _tiltZ = Mathf.SmoothDamp(_tiltZ, targetTiltZ, ref _tiltVelocityZ, _tiltSmoothTime);

            return Quaternion.Euler(_tiltX, 0, _tiltZ);
        }

        private Vector3 CalculateShake()
        {
            bool shouldShake = _movement.IsSprinting && _movement.IsMoving;

            float multiplier = shouldShake ? _shakeSprintMultiplier : 1;
            float time = Time.time * _shakeFrequency * multiplier;

            float noiseX = Mathf.PerlinNoise(time, 0) - 0.5f;
            float noiseY = Mathf.PerlinNoise(0, time) - 0.5f;

            var target = _shakeAmplitude * multiplier * new Vector3(noiseX, noiseY, 0f);

            _currentShakeOffset = Vector3.SmoothDamp(_currentShakeOffset, target, ref _shakeVelocity, _shakeSmoothTime);

            return _currentShakeOffset;
        }
    }
}
