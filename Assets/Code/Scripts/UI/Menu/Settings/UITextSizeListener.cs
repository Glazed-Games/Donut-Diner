using TMPro;
using UnityEngine;

namespace DonutDiner.UIModule.Menu.Settings
{
    // First implemented by Yuval Vinter

    public class UITextSizeListener : MonoBehaviour
    {
        #region Fields

        private TextMeshProUGUI _text;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        #endregion

        #region Public Methods

        public void ChangeSize(int size)
        {
            switch (size)
            {
                case 1:
                    _text.fontSize = 32;
                    break;

                case 2:
                    _text.fontSize = 48;
                    break;

                case 3:
                    _text.fontSize = 64;
                    break;

                default:
                    break;
            }
        }

        #endregion
    }
}