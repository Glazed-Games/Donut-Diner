using UnityEngine;

namespace DonutDiner.UIModule.Menu.Settings
{
    // First implemented by Yuval Vinter

    public class UITextHolder : MonoBehaviour
    {
        #region Fields

        [SerializeField] private UITextSizeListener[] _textsArr;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _textsArr = GameObject.Find("Canvas").GetComponentsInChildren<UITextSizeListener>(true);
        }

        #endregion

        #region Public Methods

        public void ChangeValue(int size)
        {
            foreach (var textSizeListener in _textsArr)
            {
                textSizeListener.ChangeSize(size);
            }
        }

        #endregion
    }
}