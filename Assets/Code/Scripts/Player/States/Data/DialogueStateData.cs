using DonutDiner.InteractionModule.Interactive;
using DonutDiner.ItemModule.Items;
using DonutDiner.PlayerModule.States.DTOs;
using UnityEngine;

namespace DonutDiner.PlayerModule.States.Data
{
    public class DialogueStateData : PlayerStateData
    {
        #region Properties

        public ItemToInputInto ItemToInputInto;
        public ItemToInspect ItemToInspect;
        public NPC character;
        public GameObject Prefab;

        #endregion Properties

        #region Constructor

        public DialogueStateData(PlayerActionDTO dto) : base(dto)
        {
        }

        #endregion Constructor
    }
}