using Assets.Scripts.Player.Camera;
using Assets.Scripts.Player.Input;
using Assets.Scripts.Player.Movement;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInputHandler _inputHandler;

        [Space]
        [SerializeField] private PlayerMovementController _movementController;
        [SerializeField] private PlayerFirstPersonCameraController _cameraController;

        private void Awake()
        {
            if (_inputHandler == null)
            {
                Debug.LogError($"{nameof(PlayerController)}: Input Handler is null");
                return;
            }

            if (_movementController != null) _movementController.TryInitialize(_inputHandler.Movement);
            if (_cameraController != null) _cameraController.TryInitialize(_inputHandler.Camera);
            // etc.
        }
    }
}
