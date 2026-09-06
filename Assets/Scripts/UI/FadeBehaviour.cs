using UnityEngine;

namespace Assets.Scripts.UI
{
    public class FadeBehaviour : MonoBehaviour
    {
        public bool Visible { get; private set; }

        [SerializeField, Min(0)] private float _alphaSmoothTime = 0.1f;
        [SerializeField] private CanvasGroup _canvasGroup;

        private void Awake() => Hide();
        private void Update()
        {
            if (_canvasGroup != null) _canvasGroup.alpha =
                    Mathf.MoveTowards(_canvasGroup.alpha, Visible ? 1 : 0, Time.deltaTime / _alphaSmoothTime);
        }

        public void Show() => Visible = true;
        public void Hide() => Visible = false;
    }
}
