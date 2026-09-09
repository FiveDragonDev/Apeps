using UnityEngine;

namespace Assets.Scripts
{
    public sealed class RandomColorChanger : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;

        private void Start()
        {
            ChangeToRandomColor();
        }
        public void ChangeToRandomColor()
        {
            if (_renderer == null) return;

            _renderer.material.color = Color.HSVToRGB(Random.value, 1, 1);
        }
    }
}
