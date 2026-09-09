using System;
using UnityEngine;

namespace Assets.Scripts.Player.Input.InputContext
{
    public interface IWeaponInput
    {
        event Action<bool> Fire;
    }
    public sealed class WeaponInput : IWeaponInput
    {
        public event Action<bool> Fire;

        internal void RaiseFire(bool fire) => Fire?.Invoke(fire);
    }
}
