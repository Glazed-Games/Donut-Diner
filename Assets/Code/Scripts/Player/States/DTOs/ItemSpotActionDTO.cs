using DonutDiner.InteractionModule.Environment;

namespace DonutDiner.PlayerModule.States.DTOs
{
    public class ItemSpotActionDTO : ActionDTO
    {
        #region Properties

        public ItemSpot ItemSpot { get; }

        #endregion

        #region Constructor

        public ItemSpotActionDTO(ItemSpot itemSpot)
        {
            ItemSpot = itemSpot;
        }

        #endregion
    }
}