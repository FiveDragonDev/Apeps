using System;

namespace Assets.Scripts.Player.Input.InputContext
{
    public interface IInteractionInput
    {
        public bool InteractedThisFrame { get; }
        event Action Interact;
    }
    public sealed class InteractionInput : IInteractionInput
    {
        public event Action Interact;

        public bool InteractedThisFrame { get; set; }

        internal void RaiseInteract() => Interact?.Invoke();
    }
}
