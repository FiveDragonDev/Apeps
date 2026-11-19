using UnityEngine;

namespace Assets.Scripts.Player.Movement.StateMachine.Implementation
{
    public sealed class AirborneState : PlayerMovementState
    {
        public override void Setup() => AddTransitionTo<GroundedState>(static ctx => ctx.IsGrounded);
        public override void FixedUpdate(float deltaTime)
        {
            Vector3 currentVelocity = Context.Motor.Velocity; // только физическая скорость
            currentVelocity.y += Context.Config.Gravity * deltaTime;
            currentVelocity.y = Mathf.Max(currentVelocity.y, -Context.Config.MaxFallSpeed);
            Context.Motor.Velocity = currentVelocity;

            var move = Context.Input.Move;
            var sprint = Context.Input.Sprint;
            var speed = sprint ? Context.Config.SprintSpeed : Context.Config.WalkSpeed;

            var currentHorizontal = Context.Motor.HorizontalMovement;
            if (Context.Sensor.IsOnTooSteepSlope(Context.Config.SlopeAngle))
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, Context.Sensor.GroundNormal).normalized;
                Vector3 slideTarget = slideDirection * Context.Config.SlideSpeed;

                var newHorizontal = Vector3.MoveTowards(currentHorizontal, slideTarget, Context.Config.SlideAcceleration * deltaTime);
                Context.Motor.HorizontalMovement = newHorizontal;
                return;
            }

            Vector3 forward = Context.Motor.Forward * move.y;
            Vector3 right = Context.Motor.Right * move.x;
            var direction = Vector3.ClampMagnitude(forward + right, 1);

            var horizontalAcceleration = Context.Config.AirControl * speed * direction;
            float airAcceleration = Context.Config.AirControl * Context.Config.AirAcceleration;

            var targetHorizontalVelocity = Vector3.ClampMagnitude(currentHorizontal + horizontalAcceleration * deltaTime, currentHorizontal.magnitude);

            Context.Motor.AccelerateHorizontalMovement(targetHorizontalVelocity, airAcceleration, deltaTime);
        }
    }
}
