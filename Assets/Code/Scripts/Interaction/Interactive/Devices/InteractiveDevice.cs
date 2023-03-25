using DonutDiner.FrameworkModule.Data;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive.Devices
{
    public class InteractiveDevice : SerializableObject, IInteractive
    {
        #region Fields

        [SerializeField] private bool _canInteract = true;
        [SerializeField] protected bool IsActivated;

        #endregion

        #region Properties

        public bool CanInteract { get => _canInteract; set => _canInteract = value; }

        #endregion

        #region Public Methods

        public virtual void StartInteraction()
        {
            if (!_canInteract) return;

            IsActivated = !IsActivated;
        }

        #endregion

        #region Serialization Methods

        public override void WriteData()
        {
            WriteHeader();
            Serialize.Write(IsActivated);
        }

        public override void ReadData()
        {
            ReadHeader();
            IsActivated = Serialize.ReadBool();
        }

        #endregion
    }
}