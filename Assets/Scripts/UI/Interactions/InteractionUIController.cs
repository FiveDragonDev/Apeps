using Assets.Scripts.Interactables.Base;
using Assets.Scripts.Player.Interaction;
using UnityEngine;

namespace Assets.Scripts.UI.Interactions
{
    public sealed class InteractionUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInteractionController _interaction;

        [SerializeField] private InteractionProgressView _progressView;
        [SerializeField] private FadeBehaviour _crosshair;

        private IProgressInteractable _progressInteractable;

        private Camera _camera;

        private void Start() => _camera = Camera.main;
        private void OnEnable()
        {
            _interaction.InteractionStarted += OnInteractionStarted;
            _interaction.InteractionEnded += OnInteractionEnded;

            _interaction.HoverStarted += OnHoverStarted;
            _interaction.HoverChanged += OnHoverChanged;
            _interaction.HoverEnded += OnHoverEnded;
        }
        private void OnDisable()
        {
            _interaction.InteractionStarted -= OnInteractionStarted;
            _interaction.InteractionEnded -= OnInteractionEnded;

            _interaction.HoverStarted -= OnHoverStarted;
            _interaction.HoverChanged -= OnHoverChanged;
            _interaction.HoverEnded -= OnHoverEnded;

            UnsubscribeProgress();
            _progressView.Hide();
            _crosshair.Hide();
        }
        private void Update()
        {
            if (_interaction.Hovered is { } hovered)
            {
                if (_progressView.Visible)
                {
                    _progressView.transform.position = hovered.Point + hovered.Normal * 0.1f;
                    FaceCamera(_progressView.transform);
                }

                if (_crosshair.Visible)
                {
                    _crosshair.transform.position = hovered.Point + hovered.Normal * 0.1f;
                    FaceCamera(_crosshair.transform);
                }
            }
        }

        private void OnInteractionStarted(InteractionTarget target)
        {
            if (target.Interactable is not IProgressInteractable progress) return;

            UnsubscribeProgress();

            _progressInteractable = progress;
            _progressInteractable.ProgressChanged += OnProgressChanged;

            _progressView.Show();
            _progressView.SetProgress(progress.Progress);
        }
        private void OnInteractionEnded(InteractionTarget target)
        {
            if (target.Interactable != _progressInteractable) return;

            UnsubscribeProgress();
            _progressView.Hide();
        }

        private void OnHoverStarted(InteractionTarget _) => _crosshair.Show();
        private void OnHoverChanged(InteractionTarget _, InteractionTarget __) => _crosshair.Show();
        private void OnHoverEnded(InteractionTarget target) => _crosshair.Hide();

        private void OnProgressChanged(float progress) => _progressView.SetProgress(progress);

        private void UnsubscribeProgress()
        {
            if (_progressInteractable == null) return;

            _progressInteractable.ProgressChanged -= OnProgressChanged;
            _progressInteractable = null;
        }

        private void FaceCamera(Transform transform)
        {
            if (_camera == null) return;

            var direction = transform.position - _camera.transform.position;
            if (direction.sqrMagnitude < 0.0001f) return;

            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
