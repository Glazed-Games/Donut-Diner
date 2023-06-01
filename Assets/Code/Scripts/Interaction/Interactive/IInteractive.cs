namespace DonutDiner.InteractionModule.Interactive
{
    public interface IInteractive
    {
        public bool IsInteractable();
        public void IsInteractable(bool value);
        public bool IsLocked();
        public void IsLocked(bool value);
        void StartInteraction();
    }
}