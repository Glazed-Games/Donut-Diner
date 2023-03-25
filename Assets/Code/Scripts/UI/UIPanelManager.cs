using DonutDiner.PlayerModule.States;
using UnityEngine;

namespace DonutDiner.UIModule
{
    public class UIPanelManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _hudPanel;
        [SerializeField] private GameObject _inspectionPanel;
        //[SerializeField] private GameObject _dialoguePanel;

        private static GameObject[] _panels;

        #endregion

        #region Properties

        public static bool IsOnScreen
        {
            get
            {
                foreach (GameObject panel in _panels)
                {
                    if (panel.activeSelf) return true;
                }

                return false;
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            SetReferences();
        }

        private void Start()
        {
            CloseAllPanels();
        }

        #endregion

        #region Public Methods

        public static void ToggleUIPanel(GameObject panel)
        {
            panel.SetActive(!panel.activeSelf);

            Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = panel.activeSelf;
        }

        public static void CloseOpenPanels()
        {
            foreach (GameObject panel in _panels)
            {
                if (panel.activeSelf)
                {
                    panel.SetActive(false);
                }
            }
        }

        public static void CloseAllPanels()
        {
            foreach (GameObject panel in _panels)
            {
                panel.SetActive(false);
            }
        }

        #endregion

        #region Private Methods

        private void SetReferences()
        {
            PlayerInspectState.Panel = _inspectionPanel;
            //DialogueManager.Panel = _dialoguePanel;

            _panels = new GameObject[]
            {
                PlayerInspectState.Panel,
                //DialogueManager.Panel
            };
        }

        #endregion
    }
}