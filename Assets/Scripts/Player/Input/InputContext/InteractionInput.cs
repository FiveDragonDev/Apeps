using System;

namespace Assets.Scripts.Player.Input.InputContext
{
    public interface IInteractionInput
    {
        event Action<bool> Interact;
    }
    public sealed class InteractionInput : IInteractionInput
    {
        public event Action<bool> Interact;

        internal void RaiseInteract(bool interacted) => Interact?.Invoke(interacted);
    }
}
