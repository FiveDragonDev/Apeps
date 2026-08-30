using System;
using UnityEngine;

namespace Assets.Scripts.Player.Movement
{
    [Serializable]
    public sealed class MovementConfig
    {
        [field: Header("Grounded")]
        [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float SprintSpeed { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float GroundedAcceleration { get; private set; } = 1;

        [field: Space, Header("Slope")]
        [field: SerializeField, Min(0)] public float SlopeAngle { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float SlideSpeed { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float SlideAcceleration { get; private set; } = 1;

        [field: Space, Header("Jump")]
        [field: SerializeField, Min(0)] public float JumpImpulse { get; private set; } = 1;

        [field: Space, Header("Airborne")]
        [field: SerializeField, Range(0, 1)] public float AirControl { get; private set; } = 1;
        [field: SerializeField, Min(0)] public float AirAcceleration { get; private set; } = 1;
        [field: SerializeField] public float Gravity { get; private set; } = -9.81f;
        [field: SerializeField, Min(0)] public float MaxFallSpeed { get; private set; } = 1;
    }
}
