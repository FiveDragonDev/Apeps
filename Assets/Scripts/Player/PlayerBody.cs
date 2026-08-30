using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerBody : MonoBehaviour
    {
        public float Yaw => transform.localEulerAngles.y;

        private Rigidbody _rigidbody;

        private void Start() => _rigidbody = GetComponent<Rigidbody>();

        public void SetYaw(float yaw) => _rigidbody.MoveRotation(Quaternion.Euler(0, yaw, 0));
    }
}
