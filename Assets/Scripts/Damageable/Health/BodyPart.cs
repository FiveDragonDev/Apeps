using Assets.Scripts.Definitions;
using System;
using UnityEngine;

namespace Assets.Scripts.Damageable.Health
{
    [DisallowMultipleComponent]
    public sealed class BodyPart : MonoBehaviour, IDamageable
    {
        [Serializable]
        private sealed class TissueLayer
        {
            public float Integrity => _integrity01;

            private float _integrity01 = 1;

            private readonly TissueLayerDefinition _def;

            public TissueLayer(TissueLayerDefinition definition) => _def = definition;

            public Damage AbsorbDamage(Damage damage)
            {
                if (_integrity01 == 0) return damage;

                float damageModifier = _def.GetDamageModifier(damage.HitType);
                float effectiveStrength = _def.TensileStrength * damageModifier * Integrity;

                float energyToDestroy = effectiveStrength * _def.Thickness * damage.ProjectileCrossSection; // mJ
                energyToDestroy *= 1e-3f;

                if (damage.KineticEnergy < energyToDestroy)
                {
                    float damageFraction = damage.KineticEnergy / energyToDestroy;
                    _integrity01 = Mathf.Clamp01(_integrity01 - damageFraction);
                    return Damage.None;
                }

                _integrity01 = 0;
                var targetKineticEnergy = damage.KineticEnergy - energyToDestroy;
                var newVelocity = damage.HitVelocity.normalized * Mathf.Sqrt(targetKineticEnergy * 2 / damage.ProjectileMass);
                return new(damage.HitType, newVelocity, damage.ProjectileMass, damage.ProjectileCrossSection);
            }
        }

        public event Action<TakenDamage> DamageTaken;

        [SerializeField] private BodyPart _rootPart;

        [SerializeField] private TissueLayersDefinition _layersDefinitions;

        private TissueLayer[] _layers;

        private void Awake()
        {
            _layers = new TissueLayer[_layersDefinitions.Layers.Count];
            for (int i = 0; i < _layers.Length; i++)
            {
                _layers[i] = new(_layersDefinitions.Layers[i]);
            }
        }

        public void TakeDamage(Damage damage)
        {
            var initialEnergy = damage.KineticEnergy;
            var initialHitType = damage.HitType;

            for (int i = 0; i < _layers.Length; i++)
            {
                if (damage.KineticEnergy <= Mathf.Epsilon) break;

                damage = _layers[i].AbsorbDamage(damage);
            }

            var totalAbsorbed = initialEnergy - damage.KineticEnergy;

            if (damage.KineticEnergy > 0 && _rootPart != null && _rootPart != this)
                _rootPart.TakeDamage(damage);
            
            DamageTaken?.Invoke(new(initialHitType, totalAbsorbed));
        }
    }
}
