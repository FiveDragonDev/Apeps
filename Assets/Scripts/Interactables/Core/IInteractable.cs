namespace Assets.Scripts.Interactables.Core
{
    public interface IInteractable
    {
        void BeginInteraction() { }
        void UpdateInteraction(float deltaTime) { }
        void EndInteraction() { }
    }
}
