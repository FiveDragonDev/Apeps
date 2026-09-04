using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public interface IProgressInteractable : IInteractable
    {
        float Progress { get; }
        bool IsComplete { get; }

        event Action<float> ProgressChanged;
    }
    public class ProgressHoldButton : HoldButton, IProgressInteractable
    {
        public event Action<float> ProgressChanged;

        public UnityEvent<float> ProgressUpdated;
        public UnityEvent HoldCompleted;
        public UnityEvent HoldCanceled;

        public float Progress => Mathf.Clamp01(_currentHoldTime / _requiredHoldTime);
        public float CurrentHoldTime => _currentHoldTime;
        public bool IsComplete { get; private set; }

        [SerializeField, Min(0)] private float _requiredHoldTime = 1;

        private float _currentHoldTime;

        protected override void OnBeginInteraction()
        {
            IsComplete = false;
            _currentHoldTime = 0;
        }
        protected override void OnEndInteraction()
        {
            if (!IsComplete)
            {
                _currentHoldTime = 0;
                HoldCanceled?.Invoke();
                RaiseProgressChanged(0);
            }
        }
        protected override void OnUpdateInteraction(float deltaTime)
        {
            if (!IsPressed || IsComplete) return;

            _currentHoldTime += deltaTime;

            float progress = Progress;
            if (progress >= 1)
            {
                IsComplete = true;
                _currentHoldTime = _requiredHoldTime;
                RaiseProgressChanged(1);
                HoldCompleted?.Invoke();
            }
            else RaiseProgressChanged(progress);
        }

        private void RaiseProgressChanged(float progress)
        {
            ProgressUpdated?.Invoke(progress);
            ProgressChanged?.Invoke(progress);
        }
    }
}
