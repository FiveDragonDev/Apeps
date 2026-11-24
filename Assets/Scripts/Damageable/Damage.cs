using System;
using UnityEngine;

namespace Assets.Scripts.Damageable
{
    // [Flags]
    public enum HitType : byte
    {
        None = 0,
        Sharp = 1,
        Blunt = 1 << 1,
        Tear = 1 << 2,
        Penetrating = 1 << 3,
    }
    public readonly struct Damage
    {
        public readonly float KineticEnergy; // J

        public readonly HitType HitType;
        public readonly Vector3 HitVelocity; // m/s

        public readonly float ProjectileMass; // kg
        public readonly float ProjectileCrossSection; // mm2

        public static readonly Damage None = new(HitType.None, Vector3.zero, 0, 0);

        public Damage(HitType hitType, Vector3 hitVelocity, float projectileMass, float projectileCrossSection)
        {
            HitType = hitType;
            HitVelocity = hitVelocity;
            ProjectileMass = Mathf.Max(projectileMass, Mathf.Epsilon);
            ProjectileCrossSection = Mathf.Max(projectileCrossSection, Mathf.Epsilon);
            KineticEnergy = ProjectileMass * HitVelocity.sqrMagnitude * 0.5f;
        }
    }
    public readonly struct TakenDamage
    {
        public readonly float KineticEnergy; // J
        public readonly HitType HitType;

        public TakenDamage(HitType hitType, float kineticEnergy)
        {
            KineticEnergy = kineticEnergy;
            HitType = hitType;
        }
    }
}
