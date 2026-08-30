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
        private bool _jumpRequested;

        public override void Setup() => AddTransitionTo<AirborneState>(ctx => !ctx.Sensor.IsOnSurface);
        public override void Enter() => Context.Input.Jump += OnJump;
        public override void FixedUpdate(float deltaTime)
        {
            var move = Context.Input.Move;
            var sprint = Context.Input.Sprint;

            var speed = sprint ? Context.Config.SprintSpeed : Context.Config.WalkSpeed;

            Vector3 forward = Context.Motor.Forward * move.y;
            Vector3 right = Context.Motor.Right * move.x;

            var velocity = (forward + right).normalized * speed;

            Context.Motor.AccelerateHorizontal(velocity, Context.Config.GroundedAcceleration, deltaTime);

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
        public override void Setup() => AddTransitionTo<GroundedState>(ctx => ctx.Sensor.IsOnSurface);
    }
}
