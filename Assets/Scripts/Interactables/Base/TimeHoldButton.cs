namespace Assets.Scripts.Interactables.Base
{
    public class TimeHoldButton : HoldButton
    {
        public float HoldDuration => _holdDuration;

        private float _holdDuration;

        public override void BeginInteraction()
        {
            _holdDuration = 0;
            base.BeginInteraction();
        }
        public override void UpdateInteraction(float deltaTime)
        {
            if (IsPressed) _holdDuration += deltaTime;
        }
    }
}
