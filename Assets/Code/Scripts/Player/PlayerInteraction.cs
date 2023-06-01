using DonutDiner.InteractionModule.Interactive;
using DonutDiner.ItemModule.Items;
using System;
using UnityEngine;

namespace DonutDiner.PlayerModule
{
    public class PlayerInteraction : MonoBehaviour
    {
        #region Delegates

        public event Action<bool, bool> OnExamination = delegate { };

        #endregion Delegates

        #region Fields

        [SerializeField] private float _distanceToExamine = 10.0f;
        [SerializeField] private float _distanceToInteract = 3.0f;
        [SerializeField] private LayerMask _layersToExamine;

        private Transform _examinationObject;
        private Transform _interactionObject;

        #endregion Fields

        #region Properties

        public Transform Interaction => _interactionObject;

        #endregion Properties

        #region Public Methods

        public void Examine()
        {
            if (CanExamine(out RaycastHit hit))
            {
                SetExamination(hit);

                bool canInteract = TrySetInteraction(hit);

                OnExamination(true, canInteract);
            }
            else if (_examinationObject)
            {
                ClearData();
            }
        }

        public bool TryGetInteractive(out IInteractive interactive)
        {
            interactive = null;

            if (!_interactionObject) return false;

            return _interactionObject.TryGetComponent(out interactive);
        }

        public bool TryGetItem(out IItem item)
        {
            item = null;

            if (!_interactionObject) return false;

            return _interactionObject.TryGetComponent(out item);
        }

        #endregion Public Methods

        #region Private Methods

        private void SetExamination(RaycastHit hit)
        {
            if (hit.transform != _examinationObject)
            {
                _examinationObject = hit.transform;
            }
        }

        private void SetInteraction(RaycastHit hit)
        {
            if (hit.transform != _interactionObject)
            {
                _interactionObject = _examinationObject;
            }
        }

        private bool TrySetInteraction(RaycastHit hit)
        {
            if (CanInteract(hit))
            {
                SetInteraction(hit);

                return true;
            }
            else if (_interactionObject)
            {
                _interactionObject = null;
            }

            return false;
        }

        private void ClearData()
        {
            _examinationObject = null;
            _interactionObject = null;

            OnExamination(false, false);
        }

        private bool CanExamine(out RaycastHit hit)
        {
            return Physics.Raycast(Player.Sight.position,
                                   Player.Sight.forward,
                                   out hit,
                                   _distanceToExamine,
                                   _layersToExamine,
                                   QueryTriggerInteraction.Collide) && hit.transform;
        }

        private bool CanInteract(RaycastHit hit)
        {
            return Vector3.Distance(Player.Transform.position, hit.point) <= _distanceToInteract;
        }

        #endregion Private Methods
    }
}