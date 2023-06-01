using DonutDiner.PlayerModule.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DonutDiner.UIModule
{
    public class UIPanelManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject _hudPanel;
        [SerializeField] private GameObject _inspectionPanel;
        [SerializeField] private GameObject _inputTextObj;
        //[SerializeField] private GameObject _dialoguePanel;

        private static GameObject[] _panels;

        #endregion Fields

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

        #endregion Properties

        #region Unity Methods

        private void Awake()
        {
            SetReferences();
        }

        private void Start()
        {
            CloseAllPanels();
        }

        #endregion Unity Methods

        #region Public Methods

        public static void ToggleUIPanel(GameObject panel)
        {
            //clear the focus so the event system can refocus the same element if open/closing the same item multiple times
            EventSystem.current.SetSelectedGameObject(null);
            panel.SetActive(!panel.activeSelf);

            Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = panel.activeSelf;
        }

        public static void EnableTextInput(GameObject TextInput)
        {
            if (TextInput == null) return;
            TextInput.gameObject.SetActive(true);
            if (TextInput.GetComponent<InputField>())
            {
                TextInput.GetComponent<InputField>().text = "";
                EventSystem.current.SetSelectedGameObject(TextInput);
            }
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

        #endregion Public Methods

        #region Private Methods

        private void SetReferences()
        {
            PlayerInspectState.Panel = _inspectionPanel;
            PlayerInspectState.TextInput = _inputTextObj;
            //DialogueManager.Panel = _dialoguePanel;

            _panels = new GameObject[]
            {
                PlayerInspectState.Panel,
                PlayerInspectState.TextInput,
                //DialogueManager.Panel
            };
        }

        #endregion Private Methods
    }
}