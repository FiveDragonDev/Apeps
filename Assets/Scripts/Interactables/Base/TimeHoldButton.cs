namespace Assets.Scripts.Interactables.Base
{
    public class TimeHoldButton : HoldButton
    {
        public float HoldDuration => _holdDuration;

        private float _holdDuration;

        protected override void OnBeginInteraction() => _holdDuration = 0;
        protected override void OnUpdateInteraction(float deltaTime)
        {
            if (IsPressed) _holdDuration += deltaTime;
        }
    }
}
