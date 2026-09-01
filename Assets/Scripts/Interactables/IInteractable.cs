namespace Assets.Scripts.Interactables
{
    public interface IInteractable
    {
        void BeginInteraction() { }
        void UpdateInteraction(float deltaTime) { }
        void EndInteraction() { }
    }
}
