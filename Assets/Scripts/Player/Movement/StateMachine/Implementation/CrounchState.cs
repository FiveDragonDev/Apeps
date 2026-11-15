using System;
using UnityEngine;

namespace Assets.Scripts.Player.Movement.StateMachine.Implementation
{
    public sealed class CrouchState : PlayerMovementState
    {
        public event Action StandRequested;
        public event Action StandCancelled;

        private bool _standRequested;

        public override void Setup()
        {
            AddTransitionTo<AirborneState>(static ctx => !ctx.IsGrounded);
            AddTransitionTo<GroundedState>(_ => _standRequested && Context.Sensor.CanStand);
        }

        public override void Enter()
        {
            Context.Body.SetHeight(Context.Config.CrouchHeight);

            Context.Input.Jump += RequestStand;
            Context.Input.Crouch += RequestStand;
        }
        public override void Update(float deltaTime)
        {
            if (_standRequested && !Context.Sensor.CanStand)
            {
                _standRequested = false;
                StandCancelled?.Invoke();
            }
        }
        public override void FixedUpdate(float deltaTime)
        {
            var move = Context.Input.Move;

            Vector3 forward = Context.Motor.Forward * move.y;
            Vector3 right = Context.Motor.Right * move.x;
            var direction = Vector3.ClampMagnitude(forward + right, 1);
            var targetHorizontalVelocity = direction * Context.Config.CrouchSpeed;

            var groundNormal = Context.Sensor.GroundNormal;
            if (groundNormal.y > 0.001f)
            {
                float verticalVelocity = -Vector3.Dot(targetHorizontalVelocity, groundNormal) / groundNormal.y;
                var targetVelocity = targetHorizontalVelocity + Vector3.up * verticalVelocity + Context.Sensor.PlatformVelocity;
                Context.Motor.Accelerate(targetVelocity, Context.Config.GroundedAcceleration, deltaTime);
            }
            else Context.Motor.AccelerateHorizontal(targetHorizontalVelocity, Context.Config.GroundedAcceleration, deltaTime);
        }
        public override void Exit()
        {
            Context.Input.Jump -= RequestStand;
            Context.Input.Crouch -= RequestStand;
            _standRequested = false;

            Context.Body.SetHeight(Context.Config.StandingHeight);
        }

        private void RequestStand()
        {
            _standRequested = true;
            StandRequested?.Invoke();
        }
    }
}
