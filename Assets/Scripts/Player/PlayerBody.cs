using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerBody : MonoBehaviour
    {
        public float Yaw => transform.localEulerAngles.y;
        public float Height => _collider != null ? _collider.height : 0;

        [SerializeField] private CapsuleCollider _collider;

        private Rigidbody _rigidbody;

        private void Start() => _rigidbody = GetComponent<Rigidbody>();

        public void SetYaw(float yaw) => _rigidbody.MoveRotation(Quaternion.Euler(0, yaw, 0));
        public void SetHeight(float height)
        {
            if (_collider == null) return;
            _collider.height = height;
            _collider.center = new Vector3(0, height / 2, 0);
        }
    }
}
