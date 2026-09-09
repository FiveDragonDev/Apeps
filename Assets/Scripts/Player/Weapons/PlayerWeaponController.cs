using Assets.Scripts.Player.Input.InputContext;
using Assets.Scripts.Weapons.Base;
using Assets.Scripts.Weapons.Base.Components;
using UnityEngine;

namespace Assets.Scripts.Player.Weapons
{
    public sealed class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private Gun _currentGun;

        private IWeaponInput _input;

        private void Start()
        {
            // TEST
            Magazine magazine = new(120);

            for (int i = 0; i < magazine.Capacity; i++)
            {
                _ = magazine.TryInsert(new());
            }

            _currentGun.TryAttachMagazine(magazine);
        }
        private void OnEnable()
        {
            if (_input != null) _input.Fire += OnFire;
        }
        private void OnDisable()
        {
            if (_input != null) _input.Fire -= OnFire;
        }

        public void Initialize(IWeaponInput input)
        {
            _input = input;
            if (_input != null) _input.Fire += OnFire;
        }

        private void OnFire(bool pressed)
        {
            if (_currentGun == null) return;

            if (pressed) _currentGun.OnTriggerPressed();
            else _currentGun.OnTriggerReleased();
        }
    }
}
