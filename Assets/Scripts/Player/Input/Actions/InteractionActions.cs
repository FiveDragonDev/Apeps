using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Input.Actions
{
    [Serializable]
    public sealed class InteractionActions
    {
        [field: SerializeField] public InputActionReference Interact { get; private set; }
    }
}
