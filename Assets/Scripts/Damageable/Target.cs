using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Damageable
{
    public sealed class Target : MonoBehaviour, IDamageable
    {
        public event Action<float> DamageTaken;

        [SerializeField] private UnityEvent<float> _damageTaken;

        public void TakeDamage(float damage)
        {
            if (damage <= 0) return;

            RaiseDamageEvent(damage);
        }

        private void RaiseDamageEvent(float damage)
        {
            DamageTaken?.Invoke(damage);
            _damageTaken?.Invoke(damage);
        }
    }
}
