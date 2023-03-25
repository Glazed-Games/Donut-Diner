using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DonutDiner.UIModule.Menu.Settings
{
    // First implemented by Yuval Vinter

    public class UIChangedValueHandler : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _text;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        #endregion

        #region Public Methods

        public void ChangeValue(Slider slider)
        {
            _text.text = slider.value.ToString();
        }

        #endregion
    }
}