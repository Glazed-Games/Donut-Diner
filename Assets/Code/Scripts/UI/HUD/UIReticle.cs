using DonutDiner.PlayerModule;
using UnityEngine;
using UnityEngine.UI;

namespace DonutDiner.UIModule.HUD
{
    // First implemented by Marcelo de Souza Camargo

    public class UIReticle : MonoBehaviour
    {
        #region Fields

        private Image _reticleImage;
        private PlayerInteraction _playerInteraction;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _reticleImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            if (Player.Transform.TryGetComponent(out PlayerInteraction playerExamine))
            {
                _playerInteraction = playerExamine;
                _playerInteraction.OnExamination += SetReticle;
            }
        }

        private void OnDisable()
        {
            if (_playerInteraction)
            {
                _playerInteraction.OnExamination -= SetReticle;
            }
        }

        #endregion

        #region Private Methods

        private void SetReticle(bool canExamine, bool canInteract)
        {
            _reticleImage.color = canExamine ? canInteract ? Color.green : Color.red : Color.white;
        }

        #endregion
    }
}