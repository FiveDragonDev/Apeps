using UnityEngine;

namespace Assets.Scripts.Player.Movement.StateMachine.Implementation
{
    public sealed class GroundedState : PlayerMovementState
    {
        private Vector2 _move;
        private bool _sprint;

        private bool _jumpRequested;
        private bool _crouchRequested;

        public override void Setup()
        {
            AddTransitionTo<AirborneState>(static ctx => !ctx.IsGrounded);
            AddTransitionTo<CrouchState>(_ => _crouchRequested);
        }
        public override void Enter()
        {
            Context.Input.Jump += OnJump;
            Context.Input.Crouch += OnCrouch;
        }
        public override void Update(float deltaTime)
        {
            _move = Context.Input.Move;
            _sprint = Context.Input.Sprint;
        }
        public override void FixedUpdate(float deltaTime)
        {
            var speed = _sprint ? Context.Config.SprintSpeed : Context.Config.WalkSpeed;

            Vector3 forward = Context.Motor.Forward * _move.y;
            Vector3 right = Context.Motor.Right * _move.x;
            var direction = Vector3.ClampMagnitude(forward + right, 1);
            var targetHorizontalVelocity = direction * speed;

            var groundNormal = Context.Sensor.GroundNormal;
            if (groundNormal.y > 0.001f)
            {
                float verticalVelocity = -Vector3.Dot(targetHorizontalVelocity, groundNormal) / groundNormal.y;
                var targetVelocity = targetHorizontalVelocity + Vector3.up * verticalVelocity;
                Context.Motor.Accelerate(targetVelocity, Context.Config.GroundedAcceleration, deltaTime);
            }
            else Context.Motor.AccelerateHorizontal(targetHorizontalVelocity, Context.Config.GroundedAcceleration, deltaTime);

            if (_jumpRequested)
            {
                var jumpDirection = (Vector3.up + Context.Sensor.GroundNormal).normalized;
                Context.Motor.AddImpulse(jumpDirection * Context.Config.JumpImpulse);
                _jumpRequested = false;
            }
        }
        public override void Exit()
        {
            Context.Input.Jump -= OnJump;
            Context.Input.Crouch -= OnCrouch;
            _crouchRequested = false;
        }

        private void OnJump() => _jumpRequested = true;
        private void OnCrouch() => _crouchRequested = true;
    }
}
