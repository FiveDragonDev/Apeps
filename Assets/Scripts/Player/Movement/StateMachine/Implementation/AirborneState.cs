using UnityEngine;

namespace Assets.Scripts.Player.Movement.StateMachine.Implementation
{
    public sealed class AirborneState : PlayerMovementState
    {
        private Vector2 _move;
        private bool _sprint;

        public override void Setup() => AddTransitionTo<GroundedState>(static ctx => ctx.IsGrounded);
        public override void Update(float deltaTime)
        {
            _move = Context.Input.Move;
            _sprint = Context.Input.Sprint;
        }
        public override void FixedUpdate(float deltaTime)
        {
            Vector3 currentVelocity = Context.Motor.Velocity;
            currentVelocity.y += Context.Config.Gravity * deltaTime;

            currentVelocity.y = Mathf.Max(currentVelocity.y, -Context.Config.MaxFallSpeed);
            Context.Motor.Velocity = currentVelocity;

            var speed = _sprint ? Context.Config.SprintSpeed : Context.Config.WalkSpeed;

            Vector3 forward = Context.Motor.Forward * _move.y;
            Vector3 right = Context.Motor.Right * _move.x;
            var direction = Vector3.ClampMagnitude(forward + right, 1);

            var targetHorizontalVelocity = Context.Config.AirControl * speed * direction;

            if (Context.Sensor.IsOnTooSteepSlope(Context.Config.SlopeAngle))
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, Context.Sensor.GroundNormal).normalized;
                Vector3 slideTarget = slideDirection * Context.Config.SlideSpeed;

                var currentHorizontal = Context.Motor.HorizontalVelocity;
                var newHorizontal = Vector3.MoveTowards(currentHorizontal, slideTarget, Context.Config.SlideAcceleration * deltaTime);
                Context.Motor.HorizontalVelocity = newHorizontal;
                return;
            }

            float airAcceleration = Context.Config.AirControl * Context.Config.AirAcceleration;
            Context.Motor.AccelerateHorizontal(targetHorizontalVelocity, airAcceleration, deltaTime);
        }
    }
}
