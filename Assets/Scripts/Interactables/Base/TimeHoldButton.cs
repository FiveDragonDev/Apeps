namespace Assets.Scripts.Interactables.Base
{
    public class TimeHoldButton : HoldButton
    {
        public float ActiveTime => _activeTime;

        private float _activeTime;

        public override void BeginInteraction()
        {
            _activeTime = 0;
            base.BeginInteraction();
        }
        public override void UpdateInteraction(float deltaTime)
        {
            if (IsPressed) _activeTime += deltaTime;
        }
    }
}
