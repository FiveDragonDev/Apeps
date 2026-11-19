using System;
using System.Collections.Generic;

namespace Assets.Scripts.Player.Movement.StateMachine
{
    public sealed class MovementStateMachine
    {
        public event Action<PlayerMovementState, PlayerMovementState> StateChanged; // previous (may be null) | current

        public PlayerMovementState CurrentState { get; private set; }
        public MovementContext Context { get; }

        private readonly Dictionary<Type, PlayerMovementState> _states = new();

        public MovementStateMachine(MovementContext context) => Context = context;

        public void RegisterState<T>() where T : PlayerMovementState, new()
        {
            var type = typeof(T);
            if (_states.ContainsKey(type)) return;
            _states.Add(type, new T());
        }

        public void Initialize()
        {
            foreach (var state in _states.Values) state.Initialize(this);
        }

        public bool TryGetState<T>(out T state) where T : PlayerMovementState
        {
            if (_states.TryGetValue(typeof(T), out var value))
            {
                state = value as T;
                return state != null;
            }

            state = null;
            return false;
        }

        public void SetInitialState<T>() where T : PlayerMovementState
        {
            if (CurrentState != null) return;
            if (!TryGetState(out T state)) return;

            CurrentState = state;
            CurrentState.Enter();
        }

        public void Update(float dt)
        {
            CurrentState?.Update(dt);

            if (CurrentState == null) return;
            foreach (var transition in CurrentState.Transitions)
            {
                if (!transition.Evaluate(Context)) continue;

                ChangeTo(transition.Next);
                break;
            }
        }
        public void FixedUpdate(float dt)
        {
            Context?.Motor.ReadFromRigidbody();
            CurrentState?.FixedUpdate(dt);
            Context?.Motor.ApplyToRigidbody();
        }

        private void ChangeTo(PlayerMovementState next)
        {
            if (next == null || next.Equals(CurrentState)) return;

            var previous = CurrentState;

            previous?.Exit();
            CurrentState = next;
            CurrentState.Enter();

            StateChanged?.Invoke(previous, CurrentState);
        }
    }
}
