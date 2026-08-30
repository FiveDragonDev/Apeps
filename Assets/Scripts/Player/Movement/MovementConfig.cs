using System;
using UnityEngine;

namespace Assets.Scripts.Player.Movement
{
    [Serializable]
    public sealed class MovementConfig
    {
        // [field: Header("Grounded")]
        [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float SprintSpeed { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float GroundedAcceleration { get; private set; } = 1;

        [field: Space, SerializeField, Min(0)] public float JumpImpulse { get; private set; } = 1;
    }
}
