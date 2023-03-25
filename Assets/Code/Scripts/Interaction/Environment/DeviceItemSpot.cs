using DonutDiner.InteractionModule.Interactive.Devices;
using UnityEngine;

namespace DonutDiner.InteractionModule.Environment
{
    public class DeviceItemSpot : ItemSpot
    {
        #region Fields

        [SerializeField] private InteractiveDevice _linkedDevice;

        #endregion

        #region Public Methods

        public override bool TryPlaceItem(Transform transform)
        {
            if (!base.TryPlaceItem(transform)) return false;

            _linkedDevice.StartInteraction();

            return true;
        }

        #endregion

        #region Protected Methods

        protected override void RemoveItem()
        {
            base.RemoveItem();

            _linkedDevice.StartInteraction();
        }

        #endregion
    }
}