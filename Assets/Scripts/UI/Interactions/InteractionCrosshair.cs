using UnityEngine;

namespace Assets.Scripts.UI.Interactions
{
    public sealed class InteractionCrosshair : MonoBehaviour
    {
        public bool Visible => _desiredAlpha > 0;

        [SerializeField, Min(0)] private float _alphaSmoothTime = 0.1f;

        [Space, Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;

        private float _desiredAlpha;

        private void Awake() => Hide();
        private void Update()
        {
            if (_canvasGroup != null)
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, _desiredAlpha, Time.deltaTime / _alphaSmoothTime);
        }

        public void Show() => _desiredAlpha = 1;
        public void Hide() => _desiredAlpha = 0;
    }
}
