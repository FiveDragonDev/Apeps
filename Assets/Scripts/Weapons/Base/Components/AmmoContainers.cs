using System;

namespace Assets.Scripts.Weapons.Base.Components
{
    public interface IAmmoContainer
    {
        event Action<int, int> AmmoChanged;

        bool HasAmmo { get; }

        int Capacity { get; }
        int Count { get; }

        Ammo? PeekAmmo();

        bool TryInsert(Ammo ammo);
        bool TryRemove(out Ammo ammo);
    }

    public sealed class Magazine : IAmmoContainer
    {
        public event Action<int, int> AmmoChanged;

        public bool HasAmmo => Count > 0;

        public int Capacity => _ammo.Length;
        public int Count { get; private set; }

        private readonly Ammo[] _ammo;

        public Magazine(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            _ammo = new Ammo[capacity];
            Count = 0;
        }

        public Ammo? PeekAmmo() => Count == 0 ? null : _ammo[Count - 1];

        public bool TryInsert(Ammo ammo)
        {
            if (Count >= Capacity) return false;

            var previousCount = Count;
            _ammo[Count++] = ammo;
            AmmoChanged?.Invoke(previousCount, Count);

            return true;
        }
        public bool TryRemove(out Ammo ammo)
        {
            if (Count == 0)
            {
                ammo = default;
                return false;
            }

            var previousCount = Count;
            ammo = _ammo[--Count];
            AmmoChanged?.Invoke(previousCount, Count);

            return true;
        }
    }

    public sealed class Chamber : IAmmoContainer
    {
        public event Action<int, int> AmmoChanged;

        public bool HasAmmo => _ammo.HasValue;

        public int Capacity => 1;
        public int Count => _ammo.HasValue ? 1 : 0;

        private Ammo? _ammo;

        public Ammo? PeekAmmo() => _ammo;

        public bool TryInsert(Ammo ammo)
        {
            if (HasAmmo) return false;
            _ammo = ammo;
            AmmoChanged?.Invoke(0, 1);
            return true;
        }
        public bool TryRemove(out Ammo ammo)
        {
            if (!HasAmmo)
            {
                ammo = default;
                return false;
            }

            ammo = _ammo.Value;
            _ammo = null;
            AmmoChanged?.Invoke(1, 0);
            return true;
        }
    }
}
