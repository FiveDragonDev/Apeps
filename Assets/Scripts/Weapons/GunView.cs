using Assets.Scripts.Weapons.Base;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public sealed class GunView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private LineRenderer _aimLine;
        [SerializeField] private GameObject _hitDecal;

        [Space]
        [SerializeField] private Gun _gun;
        [SerializeField] private Transform _firepoint;

        private void OnEnable()
        {
            _gun.Fired += OnFire;
            _gun.Hit += OnHit;
        }
        private void OnDisable()
        {
            _gun.Fired -= OnFire;
            _gun.Hit -= OnHit;
        }
        private void Update()
        {
            if (_aimLine != null && _firepoint != null)
            {
                Vector3 point = new(0, 0, _gun.Definition.FireRange);
                if (Physics.Raycast(_firepoint.position, _firepoint.forward, out var hit, _gun.Definition.FireRange))
                    point = transform.InverseTransformPoint(hit.point);
                _aimLine.SetPosition(1, point);
            }
        }

        private void OnFire(Ammo _)
        {
            if (_particles != null) _particles.Play(true);
        }
        private void OnHit(ShotHit hit)
        {
            if (_hitDecal == null) return;
            _ = Instantiate(_hitDecal, hit.Point, Quaternion.LookRotation(hit.Normal), hit.Collider.transform);
        }
    }
}
