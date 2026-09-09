using System;

namespace Assets.Scripts.Damageable
{
    public interface IDamageable
    {
        event Action<float> DamageTaken;

        void TakeDamage(float damage);
    }
}
