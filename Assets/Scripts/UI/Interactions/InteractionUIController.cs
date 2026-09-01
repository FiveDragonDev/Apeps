using Assets.Scripts.Interactables.Base;
using Assets.Scripts.Player.Interaction;
using UnityEngine;

namespace Assets.Scripts.UI.Interactions
{
    public sealed class InteractionUIController : MonoBehaviour
    {
        [SerializeField] private PlayerInteractionController _interaction;
        [SerializeField] private InteractionProgressView _progressView;

        private IProgressInteractable _progressInteractable;

        private Camera _camera;

        private void Start() => _camera = Camera.main;
        private void OnEnable()
        {
            _interaction.InteractionStarted += OnInteractionStarted;
            _interaction.InteractionEnded += OnInteractionEnded;
        }
        private void OnDisable()
        {
            _interaction.InteractionStarted -= OnInteractionStarted;
            _interaction.InteractionEnded -= OnInteractionEnded;

            UnsubscribeProgress();
            _progressView.Hide();
        }
        private void Update()
        {
            if (!_progressView.Visible) return;

            FaceCamera();
        }

        private void OnInteractionStarted(InteractionTarget target)
        {
            if (target.Interactable is not IProgressInteractable progress) return;

            UnsubscribeProgress();

            _progressInteractable = progress;
            _progressInteractable.ProgressChanged += OnProgressChanged;

            _progressView.SetPosition(target.Point + target.Normal * 0.1f);

            _progressView.SetProgress(progress.Progress);
            _progressView.Show();
        }
        private void OnInteractionEnded(InteractionTarget target)
        {
            if (target.Interactable != _progressInteractable) return;

            UnsubscribeProgress();
            _progressView.Hide();
        }

        private void OnProgressChanged(float progress) => _progressView.SetProgress(progress);

        private void UnsubscribeProgress()
        {
            if (_progressInteractable == null) return;

            _progressInteractable.ProgressChanged -= OnProgressChanged;
            _progressInteractable = null;
        }

        private void FaceCamera()
        {
            if (_camera == null) return;

            var direction = _progressView.transform.position - _camera.transform.position;
            if (direction.sqrMagnitude < 0.0001f) return;

            _progressView.SetRotation(Quaternion.LookRotation(direction));
        }
    }
}
