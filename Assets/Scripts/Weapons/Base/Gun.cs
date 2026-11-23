using Assets.Scripts.Damageable;
using Assets.Scripts.Definitions;
using Assets.Scripts.Weapons.Base.Components;
using System;
using UnityEngine;

namespace Assets.Scripts.Weapons.Base
{
    public enum FireType : byte { Auto = 0, SemiAuto, BoltAction }

    public readonly struct ShotHit
    {
        public readonly Ammo Ammo;

        public readonly Vector3 Point;
        public readonly Vector3 Normal;
        public readonly Collider Collider;

        public ShotHit(Ammo ammo, Vector3 point, Vector3 normal, Collider collider)
        {
            Ammo = ammo;
            Point = point;
            Normal = normal;
            Collider = collider;
        }
    }

    public class Gun : MonoBehaviour
    {
        public event Action<Ammo> Fired;
        public event Action<ShotHit> Hit;

        public event Action<IAmmoContainer, IAmmoContainer> MagazineChanged; // previous | current

        public Chamber Chamber { get; } = new();
        public IAmmoContainer Magazine { get; private set; }

        [field: SerializeField] public GunDefinition Definition { get; private set; }

        [Space, SerializeField] private Transform _firepoint;

        private float _nextFireTime = -1;
        private bool _isTriggerPressed;

        private void Update()
        {
            if (_isTriggerPressed && Definition.FireType == FireType.Auto) TryFireNow();
        }

        public bool TryAttachMagazine(IAmmoContainer magazine)
        {
            if (Magazine != null || magazine == null) return false;

            var previous = Magazine;
            Magazine = magazine;
            MagazineChanged?.Invoke(previous, Magazine);
            LoadFromMagazine();
            return true;
        }
        public bool TryDetachMagazine(out IAmmoContainer magazine)
        {
            if (Magazine == null)
            {
                magazine = null;
                return false;
            }

            magazine = Magazine;
            Magazine = null;
            MagazineChanged?.Invoke(magazine, null);
            return true;
        }

        public void OnTriggerPressed()
        {
            _isTriggerPressed = true;
            TryFireNow();
        }
        public void OnTriggerReleased() => _isTriggerPressed = false;

        public virtual void OnSelect() { }
        public virtual void OnDeselect() { }

        public bool TryClearChamber(out Ammo ammo)
        {
            if (!Chamber.TryRemove(out ammo)) return false;
            if (Definition.FireType == FireType.Auto || Definition.FireType == FireType.SemiAuto) LoadFromMagazine();
            return true;
        }

        public void LoadToChamber(Ammo ammo) => _ = Chamber.TryInsert(ammo);
        public void LoadFromMagazine()
        {
            if (Magazine == null) return;
            if (!Magazine.TryRemove(out var ammo)) return;
            LoadToChamber(ammo);
        }

        private void TryFireNow()
        {
            if (_nextFireTime >= Time.time) return;

            if (!TryClearChamber(out var ammo)) return;

            Fire(ammo);
            _nextFireTime = Time.time + 1f / Definition.FireRate;
        }

        protected virtual void Fire(Ammo ammo)
        {
            if (Physics.Raycast(_firepoint.position, _firepoint.forward, out var hit, Definition.FireRange))
            {
                if (hit.collider.TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(new(HitType.Penetrating, Vector3.forward, Definition.KineticEnergy, 1));

                Hit?.Invoke(new(ammo, hit.point, hit.normal, hit.collider));
            }

            Fired?.Invoke(ammo);
        }
    }
}
