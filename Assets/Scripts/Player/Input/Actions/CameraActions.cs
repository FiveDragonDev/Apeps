using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Input.Actions
{
    [Serializable]
    public sealed class CameraActions
    {
        [field: SerializeField] public InputActionReference Look { get; private set; }
    }
}
