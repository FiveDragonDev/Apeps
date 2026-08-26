using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player.Inputs.SOs
{
    public abstract class InputReader : ScriptableObject
    {
        protected sbyte _enableCount;

        public abstract void Enable();
        public abstract void Disable();

        protected void Subscribe(InputActionReference reference, Action<InputAction.CallbackContext> started,
            Action<InputAction.CallbackContext> performed, Action<InputAction.CallbackContext> canceled, string paramName)
        {
            if (!ValidateReference(reference, paramName)) return;
            var action = reference.action;
            if (started != null) action.started += started;
            if (performed != null) action.performed += performed;
            if (canceled != null) action.canceled += canceled;
        }
        protected void Unsubscribe(InputActionReference reference, Action<InputAction.CallbackContext> started,
            Action<InputAction.CallbackContext> performed, Action<InputAction.CallbackContext> canceled, string paramName)
        {
            if (!ValidateReference(reference, paramName)) return;
            var action = reference.action;
            if (started != null) action.started -= started;
            if (performed != null) action.performed -= performed;
            if (canceled != null) action.canceled -= canceled;
        }

        protected void EnableAction(InputActionReference reference, string paramName)
        {
            if (ValidateReference(reference, paramName)) reference.action.Enable();
        }
        protected void DisableAction(InputActionReference reference, string paramName)
        {
            if (ValidateReference(reference, paramName)) reference.action.Disable();
        }

        private bool ValidateReference(InputActionReference reference, string paramName)
        {
            if (reference == null)
            {
                Debug.LogError($"Input Reader {name}: {paramName} is not assigned", this);
                return false;
            }
            if (reference.action == null)
            {
                Debug.LogError($"Input Reader {name}: {paramName}.action is null", this);
                return false;
            }
            return true;
        }
    }
}
