using DonutDiner.InteractionModule.Interactive;

namespace DonutDiner.PlayerModule.States.DTOs
{
    public class InteractiveActionDTO : ActionDTO
    {
        #region Properties

        public IInteractive Interactive { get; }

        #endregion Properties

        #region Constructor

        public InteractiveActionDTO(IInteractive interactive)
        {
            Interactive = interactive;
        }

        #endregion Constructor
    }
}