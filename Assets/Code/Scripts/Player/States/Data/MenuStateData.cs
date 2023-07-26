using DonutDiner.InteractionModule.Interactive;
using DonutDiner.ItemModule.Items;
using DonutDiner.PlayerModule.States.DTOs;
using UnityEngine;

namespace DonutDiner.PlayerModule.States.Data
{
    public class MenuStateData : PlayerStateData
    {
        #region Properties
        public ItemToCarry ItemToCarry;

        #endregion Properties

        #region Constructor

        public MenuStateData(PlayerActionDTO dto) : base(dto)
        {
        }

        #endregion Constructor
    }
}