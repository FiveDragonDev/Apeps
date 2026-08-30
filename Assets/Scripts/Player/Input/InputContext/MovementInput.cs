using System;
using UnityEngine;

namespace Assets.Scripts.Player.Input.InputContext
{
    public interface IMovementInput
    {
        event Action Jump;

        Vector2 Move { get; }
        bool Sprint { get; }
    }
    public sealed class MovementInput : IMovementInput
    {
        public event Action Jump;

        public Vector2 Move { get; private set; }
        public bool Sprint { get; private set; }

        internal void RaiseJump() => Jump?.Invoke();
        internal void SetMove(Vector2 move) => Move = move;
        internal void SetSprint(bool sprint) => Sprint = sprint;
    }
}
