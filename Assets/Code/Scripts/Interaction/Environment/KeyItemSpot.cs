using DonutDiner.InteractionModule.Interactive.Devices;
using UnityEngine;

namespace DonutDiner.InteractionModule.Environment
{
    public class KeyItemSpot : ItemSpot
    {
        #region Fields

        [SerializeField] private InteractiveDevice _linkedDevice;

        #endregion

        #region Public Methods

        public override bool TryPlaceItem(Transform transform)
        {
            if (!base.TryPlaceItem(transform)) return false;

           
            _linkedDevice.CanInteract = true;

            return true;
        }

        public bool HasKey()
        {
            if (Item) { return true; }
            return false;
        }

        #endregion

        #region Protected Methods

        protected override void RemoveItem()
        {
            base.RemoveItem();

            _linkedDevice.CanInteract = false;
           // _linkedDevice.StartInteraction();
        }

        #endregion
    }
}