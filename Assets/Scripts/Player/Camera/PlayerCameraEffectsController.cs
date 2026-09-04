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

        private MovementContext _movement;
        private ICameraInput _input;

        private float _bobTimer;

        private Vector2 _tilt;
        private Vector2 _tiltVelocity;

        private Vector3 _currentShakeOffset;
        private Vector3 _shakeVelocity;

        private void LateUpdate()
        {
            if (_movement == null || _input == null) return;

            var bob = CalculateHeadBob();
            var tilt = CalculateTilt();
            var shake = CalculateShake();

            transform.SetLocalPositionAndRotation(bob + shake, tilt);
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
            Vector2 amount = new(_walkBobAmountX, _walkBobAmountY);

            if (_movement.IsSprinting)
            {
                bobSpeed *= _sprintBobSpeedMultiplier;
                amount *= _sprintBobAmountMultiplier;
            }

            _bobTimer += Time.deltaTime * bobSpeed * speedRatio;

            return new(Mathf.Cos(_bobTimer) * amount.x, Mathf.Sin(_bobTimer * 2) * amount.y, 0);
        }

        private Quaternion CalculateTilt()
        {
            Vector2 move = _movement.Input.Move;

            Vector2 targetTilt = new(move.y * _turnTiltAngle, -move.x * _strafeTiltAngle);
            _tilt = Vector2.SmoothDamp(_tilt, targetTilt, ref _tiltVelocity, _tiltSmoothTime);

            return Quaternion.Euler(_tilt.x, 0, _tilt.y);
        }

        private Vector3 CalculateShake()
        {
            bool shouldShake = _movement.IsSprinting && _movement.IsMoving;

            float multiplier = shouldShake ? _shakeSprintMultiplier : 1;
            float time = Time.time * _shakeFrequency * multiplier;

            float noiseX = Mathf.PerlinNoise(time, 0) - 0.5f;
            float noiseY = Mathf.PerlinNoise(0, time) - 0.5f;

            var target = _shakeAmplitude * multiplier * new Vector3(noiseX, noiseY, 0);

            _currentShakeOffset = Vector3.SmoothDamp(_currentShakeOffset, target, ref _shakeVelocity, _shakeSmoothTime);

            return _currentShakeOffset;
        }
    }
}
