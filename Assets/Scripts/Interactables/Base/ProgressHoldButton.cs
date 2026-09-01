using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public class ProgressHoldButton : HoldButton
    {
        public UnityEvent<float> ProgressUpdated;
        public UnityEvent HoldCompleted;
        public UnityEvent HoldCanceled;

        public float Progress => Mathf.Clamp01(_currentHoldTime / _requiredHoldTime);
        public float CurrentHoldTime => _currentHoldTime;
        public bool IsComplete { get; private set; }

        [SerializeField, Min(0)] private float _requiredHoldTime = 1;

        private float _currentHoldTime;

        public void ResetProgress()
        {
            _currentHoldTime = 0;
            IsComplete = false;
            ProgressUpdated?.Invoke(0);
        }

        public override void BeginInteraction()
        {
            IsComplete = false;
            _currentHoldTime = 0;
            base.BeginInteraction();
        }
        public override void EndInteraction()
        {
            base.EndInteraction();

            if (!IsComplete)
            {
                _currentHoldTime = 0f;
                HoldCanceled?.Invoke();
                ProgressUpdated?.Invoke(0);
            }
        }
        public override void UpdateInteraction(float deltaTime)
        {
            if (!IsPressed || IsComplete) return;

            _currentHoldTime += deltaTime;

            float progress = Progress;
            ProgressUpdated?.Invoke(progress);

            if (progress >= 1)
            {
                IsComplete = true;
                _currentHoldTime = _requiredHoldTime;
                ProgressUpdated?.Invoke(1);
                HoldCompleted?.Invoke();
            }
        }
    }
}
