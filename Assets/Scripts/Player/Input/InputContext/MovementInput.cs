using System;
using UnityEngine;

namespace Assets.Scripts.Player.Input.InputContext
{
    public interface IMovementInput
    {
        event Action Jump;
        event Action Crouch;

        Vector2 Move { get; }
        bool Sprint { get; }
    }
    public sealed class MovementInput : IMovementInput
    {
        public event Action Jump;
        public event Action Crouch;

        public Vector2 Move { get; private set; }
        public bool Sprint { get; private set; }

        internal void RaiseJump() => Jump?.Invoke();
        internal void RaiseCrouch() => Crouch?.Invoke();
        internal void SetMove(Vector2 move) => Move = move;
        internal void SetSprint(bool sprint) => Sprint = sprint;
    }
}
