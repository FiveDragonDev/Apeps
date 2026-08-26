using UnityEngine;

namespace Assets.Scripts.Player.Input.Context
{
    public interface ICameraContext
    {
        Vector2 Look { get; }
    }
    public sealed class CameraContext : ICameraContext
    {
        public Vector2 Look { get; set; }
    }
}
