using Assets.Scripts.Player.Movement.Sensors;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.Movement.StateMachine
{
    public abstract class PlayerMovementState
    {
        public IReadOnlyList<MovementTransition> Transitions => _transitions;

        protected MovementStateMachine Machine => _machine;
        protected MovementContext Context => _context;

        private MovementStateMachine _machine;
        private MovementContext _context;

        private readonly List<MovementTransition> _transitions = new();

        public void Initialize(MovementStateMachine machine)
        {
            _machine = machine;
            _context = machine.Context;

            Setup();
        }

        public virtual void Setup() { }
        public virtual void Enter() { }
        public virtual void Update(float deltaTime) { }
        public virtual void FixedUpdate(float deltaTime) { }
        public virtual void Exit() { }

        protected void AddTransitionTo<T>() where T : PlayerMovementState => AddTransitionTo<T>(_ => true);
        protected void AddTransitionTo<T>(Func<MovementContext, bool> predicate) where T : PlayerMovementState
        {
            if (_machine.TryGetState(out T next)) _transitions.Add(new(next, predicate));
        }
    }

    public sealed class GroundedState : PlayerMovementState
    {
        private Vector2 _move;
        private bool _sprint;

        private bool _jumpRequested;

        public override void Setup() => AddTransitionTo<AirborneState>(ctx => !ctx.IsGrounded);
        public override void Enter() => Context.Input.Jump += OnJump;
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
            var direction = Vector3.ClampMagnitude(forward + right, 1f);
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
        public override void Exit() => Context.Input.Jump -= OnJump;

        private void OnJump() => _jumpRequested = true;
    }

    public sealed class AirborneState : PlayerMovementState
    {
        private Vector2 _move;
        private bool _sprint;

        public override void Setup() => AddTransitionTo<GroundedState>(ctx => ctx.IsGrounded);
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
            var direction = Vector3.ClampMagnitude(forward + right, 1f);

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
