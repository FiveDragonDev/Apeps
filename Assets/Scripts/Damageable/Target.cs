using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Damageable
{
    public sealed class Target : MonoBehaviour, IDamageable
    {
        public event Action<TakenDamage> DamageTaken;

        [SerializeField] private UnityEvent<TakenDamage> _damageTaken;

        public void TakeDamage(Damage damage)
        {
            if (damage.KineticEnergy <= Mathf.Epsilon) return;

            RaiseDamageEvent(new(damage.HitType, damage.KineticEnergy));
        }

        private void RaiseDamageEvent(TakenDamage damage)
        {
            DamageTaken?.Invoke(damage);
            _damageTaken?.Invoke(damage);
        }
    }
}
