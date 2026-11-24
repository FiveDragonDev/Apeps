using Assets.Scripts.Damageable;
using Assets.Scripts.Helpers;
using System;
using UnityEngine;

namespace Assets.Scripts.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/Health/Tissue Layer Definition", order = 0)]
    public sealed class TissueLayerDefinition : ScriptableObject
    {
        [Serializable] private sealed class DamageModifiersDictionary : SerializableDictionary<HitType, float> { }

        [field: SerializeField, Min(0)] public float Density { get; private set; } = 1; // g/cm3
        [field: SerializeField, Min(0)] public float Thickness { get; private set; } = 1; // mm
        [field: SerializeField, Min(0)] public float TensileStrength { get; private set; } = 1; // MPa

        [SerializeField] private DamageModifiersDictionary _damageModifiers;

        public float GetDamageModifier(HitType hitType) => _damageModifiers.TryGetValue(hitType, out var modifier) ? modifier : 1;
    }
}
