using Assets.Scripts.Damageable;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Definitions
{
    [CreateAssetMenu(menuName = "Definitions/Health/Tissue Layer Definition", order = 0)]
    public sealed class TissueLayerDefinition : ScriptableObject
    {
        [field: SerializeField, Min(0)] public float Density { get; private set; } = 1; // g/cm3
        [field: SerializeField, Min(0)] public float Thickness { get; private set; } = 1; // mm
        [field: SerializeField, Min(0)] public float TensileStrength { get; private set; } = 1; // MPa

        [SerializeField] private SerializableDictionary<HitType, float> _damageModifiers;

        public float GetDamageModifier(HitType hitType) => ((Dictionary<HitType, float>)_damageModifiers).TryGetValue(hitType, out var modifier) ? modifier : 1;
    }
}
