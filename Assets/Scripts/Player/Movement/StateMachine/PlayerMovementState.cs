using System;
using System.Collections.Generic;

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
        protected void AddTransitionTo<T>(Predicate<MovementContext> predicate) where T : PlayerMovementState
        {
            if (_machine.TryGetState(out T next)) _transitions.Add(new(next, predicate));
        }
    }
}
