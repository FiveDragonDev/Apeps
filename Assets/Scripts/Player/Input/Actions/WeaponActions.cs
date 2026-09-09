using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Input.Actions
{
    [Serializable]
    public sealed class WeaponActions
    {
        [field: SerializeField] public InputActionReference Fire { get; private set; }
    }
}
