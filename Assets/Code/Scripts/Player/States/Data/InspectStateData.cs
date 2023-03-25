using DonutDiner.ItemModule.Items;
using DonutDiner.PlayerModule.States.DTOs;
using UnityEngine;

namespace DonutDiner.PlayerModule.States.Data
{
    public class InspectStateData : PlayerStateData
    {
        #region Properties

        public ItemToInspect ItemToInspect;
        public GameObject Prefab;

        #endregion

        #region Constructor

        public InspectStateData(PlayerActionDTO dto) : base(dto)
        {
        }

        #endregion
    }
}