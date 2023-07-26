using DonutDiner.InteractionModule.Interactive.Devices;
using UnityEngine;

namespace DonutDiner.InteractionModule.Environment
{
    public class DeviceItemSpot : ItemSpot
    {
        #region Fields

        [SerializeField] private InteractiveDevice _linkedDevice;

        #endregion Fields

        #region Public Methods

        public override bool TryPlaceItem(Transform transform)
        {
            if (!base.TryPlaceItem(transform)) return false;

            if (_linkedDevice) _linkedDevice.StartInteraction();

            return true;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void RemoveItem()
        {
            base.RemoveItem();

            if (_linkedDevice) _linkedDevice.StartInteraction();
        }

        #endregion Protected Methods
    }
}