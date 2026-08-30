using UnityEngine;

namespace Assets.Scripts.Player.Input.InputContext
{
    public interface ICameraInput
    {
        Vector2 Look { get; }
    }
    public sealed class CameraInput : ICameraInput
    {
        public Vector2 Look { get; private set; }

        internal void SetLook(Vector2 look) => Look = look;
    }
}
