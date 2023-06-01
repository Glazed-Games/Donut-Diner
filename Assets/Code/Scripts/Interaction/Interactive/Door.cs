using DonutDiner.FrameworkModule.Data;
using System.Collections;
using UnityEngine;

namespace DonutDiner.InteractionModule.Interactive
{
    public class Door : SerializableObject, IInteractive
    {
        #region Fields
         
        [SerializeField] private bool _isOpen;
        [SerializeField] private bool _isLocked;

        [Header("Automatic Open / Close")]
        [Space]
        [SerializeField] private bool _isTriggered;

        [SerializeField] private bool _shouldStayOpen;
        [SerializeField] private float _secondsToClose = 0.0f;
        [SerializeField] private LayerMask _layersToTrigger;

        private Coroutine _closingDoor;
        private Animator _animator;

        #endregion Fields

        #region Properties

 

        #endregion Properties

        #region Unity Methods

        private void Awake()
        {
            SetReferences();
        }

        private void Start()
        {
            SetAnimation();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isLocked || !IsTriggered(other)) return;

            ChangeState(true);

            if (_closingDoor != null) StopCoroutine(_closingDoor);
        }

        private void OnTriggerExit(Collider other)
        {
            if (ShouldClose() && IsTriggered(other)) PrepareCoroutine();
        }

        #endregion Unity Methods

        #region Public Methods

        public void StartInteraction()
        {
            if (_isLocked) return;

            ChangeState(!_isOpen);

            if (HasTimerToClose()) PrepareCoroutine();
        }

        public void Unlock()
        {
            _isLocked = false;
        }

        public bool IsInteractable()
        {
            return _isLocked;
        }

        public void IsInteractable(bool value)
        {
            _isLocked = !value;
        }

        public bool IsLocked()
        {
            return _isLocked;
        }

        public void IsLocked(bool value)
        {
            _isLocked = !value;
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
            Serialize.Write(_isLocked);
            Serialize.Write(_secondsToClose);
        }

        public override void ReadData()
        {
            ReadHeader();
            _isOpen = Serialize.ReadBool();
            _isLocked = Serialize.ReadBool();

            SetAnimation();
        }

        #endregion Serialization Methods
    }
}