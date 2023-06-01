using DonutDiner.FrameworkModule.Data;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive.Devices
{
    public class InteractiveDevice : SerializableObject, IInteractive
    {
        #region Fields

        [SerializeField] private bool _isLocked;
        [SerializeField] private bool _canInteract = true;
        [SerializeField] protected bool IsActivated;

        #endregion Fields

        #region Properties

        public bool CanInteract { get => _canInteract; set => _canInteract = value; }

        #endregion Properties

        #region Public Methods

        public bool IsInteractable()
        {
            return _canInteract;
        }

        public void IsInteractable(bool value)
        {
            _canInteract = value;
        }

        public bool IsLocked()
        {
            return _isLocked;
        }

        public void IsLocked(bool value)
        {
            _isLocked = !value;
        }

        public virtual void StartInteraction()
        {
            if (!_canInteract) return;

            IsActivated = !IsActivated;
        }

        #endregion Public Methods

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

        #endregion Serialization Methods
    }
}