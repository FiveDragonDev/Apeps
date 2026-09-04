using System;

namespace Assets.Scripts.Player.Movement.StateMachine
{
    public readonly struct MovementTransition
    {
        public readonly PlayerMovementState Next;
        public readonly Predicate<MovementContext> Predicate;

        internal MovementTransition(PlayerMovementState next, Predicate<MovementContext> predicate)
        {
            Next = next;
            Predicate = predicate;
        }

        public readonly bool Evaluate(MovementContext context) => Predicate(context);
    }
}
