using System;

namespace Assets.Scripts.Player.Movement.StateMachine
{
    public readonly struct MovementTransition
    {
        public readonly PlayerMovementState Next;
        public readonly Func<MovementContext, bool> Predicate;

        internal MovementTransition(PlayerMovementState next, Func<MovementContext, bool> predicate)
        {
            Next = next;
            Predicate = predicate;
        }

        public readonly bool Evaluate(MovementContext context) => Predicate(context);
    }
}
