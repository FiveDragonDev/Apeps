using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Interactions
{
    public sealed class InteractionProgressView : MonoBehaviour
    {
        public bool Visible { get; private set;  }

        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _progress;

        private void Awake() => Hide();

        public void Show()
        {
            if (_canvas != null) _canvas.enabled = true;
            Visible = true;
        }
        public void Hide()
        {
            if (_canvas != null) _canvas.enabled = false;
            Visible = false;
        }

        public void SetProgress(float progress) => _progress.fillAmount = progress;
        public void SetPosition(Vector3 position) => transform.position = position;
        public void SetRotation(Quaternion rotation) => transform.rotation = rotation;
    }
}
