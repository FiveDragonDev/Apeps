using Assets.Scripts.Player.Inputs.SOs;
using UnityEngine;

namespace Assets.Scripts.Player.Inputs
{
    public sealed class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private InputReader[] _inputReaders;

        private void OnEnable()
        {
            if (_inputReaders == null || _inputReaders.Length == 0)
            {
                Debug.LogError($"{nameof(PlayerInputHandler)}: Input Readers is not assigned", this);
                return;
            }
            foreach (var inputReader in _inputReaders)
            {
                if (inputReader != null) inputReader.Enable();
                else Debug.LogError($"InputReader is null in array", this);
            }
        }
        private void OnDisable()
        {
            if (_inputReaders == null || _inputReaders.Length == 0)
            {
                Debug.LogError($"{nameof(PlayerInputHandler)}: Input Readers is not assigned", this);
                return;
            }
            
            foreach (var inputReader in _inputReaders)
            {
                if (inputReader != null) inputReader.Disable();
                else Debug.LogError($"InputReader is null in array", this);
            }
        }
    }
}
