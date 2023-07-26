using DonutDiner.FrameworkModule;
using DonutDiner.FrameworkModule.Data;
using DonutDiner.InteractionModule.Environment;
using DonutDiner.ItemModule;
using DonutDiner.PlayerModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive.Devices
{
    /* ===How the switch works===
     * 
     * Switches default to being usable and unlocked.
     * if:
     *  -Keyspot is assigned: the switch is only usable when that specific 'keyspot' has the item it requires
     *  -ItemNeeded is assigned: the switch checks that the player posses the specific item before acting
     * 
     * When the player interacts with the switch while it is unlocked, it calls 'StartInteraction' on all objects in the list of "Controlled Devices"
     * 
     */



    [ExecuteInEditMode]
    public class UsableSwitch : InteractiveDevice
    {
        #region Fields

        [SerializeField] private KeyItemSpot keySpot;
        [SerializeField] private ItemObject itemNeeded;
        [SerializeField] private List<InteractiveDevice> controlledDevices;
        [SerializeField] private bool _isOpen;

        [Header("Automatic Open / Close")]
        [Space]
        [SerializeField] private bool _isTriggered;

        [SerializeField] private bool _shouldStayOpen;
        [SerializeField] private float _secondsToClose = 0.0f;
        [SerializeField] private LayerMask _layersToTrigger;

        [Header("Debug: Editor window shows rays to connected devices")]
        [SerializeField] private bool debugShowControlledDevices;

        [SerializeField] private float debugRayLifeTime;
        [SerializeField] private Color debugRayColor = Color.green;

        private Coroutine _closingDoor;
        private Animator _animator;

        #endregion Fields

        #region Unity Methods

        private void Awake()
        {
            SetReferences();
        }

        private void Start()
        {
            SetAnimation();
        }

        private void Update()
        {
            if (debugShowControlledDevices)
            {
                foreach (InteractiveDevice el in ControlledDevices())
                {
                    if (el != null)
                    {
                        Debug.DrawRay(transform.position, el.transform.position - transform.position, debugRayColor, debugRayLifeTime);
                    }
                }
                debugShowControlledDevices = false;
            }
        }

        #endregion Unity Methods

        #region Public Methods

        public override void StartInteraction()
        {
            if (!CanInteract) return;
            if (IsLocked()) return;
            if (!UnlockConditionMet()) return;

            ChangeState(!_isOpen);

            foreach (InteractiveDevice el in ControlledDevices())
            {
                if (el != null)
                { el.StartInteraction(); }
            }

            if (HasTimerToClose()) PrepareCoroutine();
        }

        public bool UnlockConditionMet()
        {
            //check a central keyItemSpot for the required item
            if (keySpot != null && !keySpot.HasKey()) return false;
            //check the player's inventory for the required item
            if (itemNeeded != null && !PlayerInventory.Instance.CheckForItem(itemNeeded)) return false;

            //if a key spot or a needed item isnt set, the device defaults to usable
            return true;
        }

        public List<InteractiveDevice> ControlledDevices()
        {
            if (controlledDevices == null || controlledDevices.Count == 0)
            {
                controlledDevices = new List<InteractiveDevice>();
                foreach (Transform el in transform)
                {
                    InteractiveDevice newPiece = el.GetComponent<InteractiveDevice>();
                    if (newPiece != null)
                    {
                        if (!controlledDevices.Contains(newPiece))
                        {
                            controlledDevices.Add(newPiece);
                        }
                    }
                }
            }

            return controlledDevices;
        }

        #endregion Public Methods

        #region Private Methods

        private void ChangeState(bool isOpen)
        {
            _isOpen = isOpen;
            _animator.SetBool("isOpen", isOpen);
        }

        private void SetAnimation()
        {
            string layer = _isOpen ? "Open" : "Closed";

            _animator.SetBool("isOpen", _isOpen);
            _animator.SetLayerWeight(_animator.GetLayerIndex(layer), 1.0f);
        }

        private void PrepareCoroutine()
        {
            if (_closingDoor != null) StopCoroutine(_closingDoor);

            _closingDoor = StartCoroutine(WaitToClose());
        }

        private IEnumerator WaitToClose()
        {
            float deadline = Time.time + _secondsToClose;

            while (Time.time < deadline)
            {
                yield return new WaitForSeconds(1.0f);
            }

            ChangeState(false);

            yield return null;
        }

        private bool IsTriggered(Collider other)
        {
            return _isTriggered && _layersToTrigger == (_layersToTrigger | (1 << other.gameObject.layer));
        }

        private bool ShouldClose() => _isOpen && !_shouldStayOpen;

        private bool HasTimerToClose() => _secondsToClose > 0 && _isOpen;

        private void SetReferences()
        {
            _animator = GetComponent<Animator>();
        }

        #endregion Private Methods

        #region Serialization Methods

        public override void WriteData()
        {
            WriteHeader();
            Serialize.Write(_isOpen);
            Serialize.Write(IsLocked());
            Serialize.Write(_secondsToClose);
        }

        public override void ReadData()
        {
            ReadHeader();
            _isOpen = Serialize.ReadBool();
            IsLocked(Serialize.ReadBool());

            SetAnimation();
        }

        #endregion Serialization Methods
    }
}