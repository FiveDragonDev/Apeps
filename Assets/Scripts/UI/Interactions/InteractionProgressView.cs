using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Interactions
{
    public sealed class InteractionProgressView : FadeBehaviour
    {
        [SerializeField] private Gradient _gradientByProgress;
        [SerializeField] private Image _progress;

        public void SetProgress(float progress)
        {
            _progress.color = _gradientByProgress.Evaluate(progress);
            _progress.fillAmount = progress;
        }
    }
}
