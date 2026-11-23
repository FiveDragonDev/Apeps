using System;

namespace Assets.Scripts.Damageable
{
    public interface IDamageable
    {
        event Action<TakenDamage> DamageTaken;

        void TakeDamage(Damage damage);
    }
}
