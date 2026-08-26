using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Input.Actions
{
    [Serializable]
    public sealed class MovementActions
    {
        [field: SerializeField] public InputActionReference Move { get; private set; }
        [field: SerializeField] public InputActionReference Sprint { get; private set; }
        [field: SerializeField] public InputActionReference Jump { get; private set; }
    }
}
