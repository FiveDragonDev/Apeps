using Assets.Scripts.Player.Camera;
using Assets.Scripts.Player.Input;
using Assets.Scripts.Player.Interaction;
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
        [SerializeField] private PlayerInteractionController _interactionController;

        private void Awake()
        {
            if (_inputHandler == null)
            {
                Debug.LogError($"{nameof(PlayerController)}: Input Handler is null");
                return;
            }

            if (_movementController != null) _movementController.Initialize(_inputHandler.Movement);
            if (_cameraController != null) _cameraController.Initialize(_inputHandler.Camera);
            if (_interactionController != null) _interactionController.Initialize(_inputHandler.Interaction);
            // etc.
        }
    }
}
