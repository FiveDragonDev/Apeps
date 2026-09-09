using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Interactables.Base
{
    public interface IProgressInteractable : IInteractable
    {
        event Action<float> ProgressChanged;
        event Action Completed;

        float Progress { get; }
        bool IsComplete { get; }
    }
    public class ProgressHoldButton : HoldButton, IProgressInteractable
    {
        public event Action<float> ProgressChanged;
        public event Action Completed;

        public float Progress => Mathf.Clamp01(_currentHoldTime / _requiredHoldTime);
        public float CurrentHoldTime => _currentHoldTime;
        public bool IsComplete { get; private set; }

        [SerializeField] private UnityEvent<float> _progressChanged;
        [SerializeField] private UnityEvent _holdCompleted;
        [SerializeField] private UnityEvent _holdCanceled;

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
                _holdCanceled?.Invoke();
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
                RaiseCompleted();
            }
            else RaiseProgressChanged(progress);
        }

        private void RaiseProgressChanged(float progress)
        {
            _progressChanged?.Invoke(progress);
            ProgressChanged?.Invoke(progress);
        }
        private void RaiseCompleted()
        {
            Completed?.Invoke();
            _holdCompleted?.Invoke();
        }
    }
}
