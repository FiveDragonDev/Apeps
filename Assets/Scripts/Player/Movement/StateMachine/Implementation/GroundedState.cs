using System;
using UnityEngine;

namespace Assets.Scripts.Player.Movement.StateMachine.Implementation
{
    public sealed class GroundedState : PlayerMovementState
    {
        public event Action CrouchRequested;
        public event Action JumpRequested;
        public event Action Jumped;

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
        public override void FixedUpdate(float deltaTime)
        {
            var move = Context.Input.Move;
            var sprint = Context.Input.Sprint;

            var speed = sprint ? Context.Config.SprintSpeed : Context.Config.WalkSpeed;

            Vector3 forward = Context.Motor.Forward * move.y;
            Vector3 right = Context.Motor.Right * move.x;
            var direction = Vector3.ClampMagnitude(forward + right, 1);
            var targetHorizontalVelocity = direction * speed;

            var groundNormal = Context.Sensor.GroundNormal;
            if (groundNormal.y > 0.001f)
            {
                float verticalVelocity = -Vector3.Dot(targetHorizontalVelocity, groundNormal) / groundNormal.y;
                var targetVelocity = targetHorizontalVelocity + Vector3.up * verticalVelocity + Context.Sensor.PlatformVelocity;
                Context.Motor.Accelerate(targetVelocity, Context.Config.GroundedAcceleration, deltaTime);
            }
            else Context.Motor.AccelerateHorizontal(targetHorizontalVelocity, Context.Config.GroundedAcceleration, deltaTime);

            if (_jumpRequested)
            {
                var jumpDirection = (Vector3.up + Context.Sensor.GroundNormal).normalized;
                Context.Motor.AddImpulse(jumpDirection * Context.Config.JumpImpulse);
                _jumpRequested = false;
                Jumped?.Invoke();
            }
        }
        public override void Exit()
        {
            Context.Input.Jump -= OnJump;
            Context.Input.Crouch -= OnCrouch;
            _crouchRequested = false;
        }

        private void OnJump()
        {
            _jumpRequested = true;
            JumpRequested?.Invoke();
        }

        private void OnCrouch()
        {
            _crouchRequested = true;
            CrouchRequested?.Invoke();
        }
    }
}
