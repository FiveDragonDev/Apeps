using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Interactions
{
    public sealed class InteractionProgressView : FadeBehaviour
    {
        [Space]
        [SerializeField] private Color _completeColor;
        [SerializeField] private Gradient _gradientByProgress;
        [SerializeField] private Image _progress;

        public void SetProgress(float progress)
        {
            if (progress >= 1)
            {
                _progress.color = _completeColor;
                _progress.fillAmount = 1;
                return;
            }
            _progress.color = _gradientByProgress.Evaluate(progress);
            _progress.fillAmount = progress;
        }
    }
}
