using UnityEngine;

namespace DonutDiner.PlayerModule.States.DTOs
{
    public class TransformActionDTO : ActionDTO
    {
        #region Properties

        public Transform Transform { get; }

        #endregion

        #region Constructor

        public TransformActionDTO(Transform transform)
        {
            Transform = transform;
        }

        #endregion
    }
}