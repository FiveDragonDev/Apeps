using System;
using UnityEngine;

namespace Assets.Scripts.Player.Input.Context
{
    public interface IMovementContext
    {
        event Action Jump;

        Vector2 Move { get; }
        bool Sprint { get; }
    }
    public sealed class MovementContext : IMovementContext
    {
        public event Action Jump;

        public Vector2 Move { get; set; }
        public bool Sprint { get; set; }

        public void RaiseJump() => Jump?.Invoke();
    }
}
