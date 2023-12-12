using DonutDiner.FrameworkModule;
using DonutDiner.ItemModule.Items;
using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;
using DonutDiner.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace DonutDiner.PlayerModule.States
{
    public class PlayerDialogueState : PlayerBaseState
    {
        #region Fields

        [SerializeField] private float _initialFOV = 60.0f;

        public static GameObject Panel;
        public static GameObject TextInput;

        private DialogueStateData _data;

        #endregion Fields

        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            StateData = new DialogueStateData(dto);
            _data = StateData as DialogueStateData;
            EnterDialogue();
            SetItemToInspect();
            ActivateUI();
        }

        public override PlayerActionDTO ExitState()
        {
            ReadTextInput();

            DeactivateUI();

            if (_data.ItemToInspect) _data.ItemToInspect.FinishInspection();

            return StateData.GetData();
        }

        public void ReadTextInput()
        {
            if (_data.ItemToInputInto && _data.ItemToInputInto.GetComponent<ItemToInputInto>())
            {
                if (TextInput.GetComponent<InputField>())
                { _data.ItemToInputInto.GetComponent<ItemToInputInto>().GetTextInput(TextInput.GetComponent<InputField>().text); }
            }
        }

        public override void UpdateStates()
        {
        }

        public override void UpdateState()
        {
        }

        public override void UpdateCamera()
        {
        }

        public override void CheckSwitchState()
        {
        }

        public override bool TrySwitchState(ActionType action, ActionDTO dto = null)
        {
            switch (action)
            {
                case ActionType.None:
                    PopState();
                    return true;

                case ActionType.Dialogue:
                    HandleDialogue();
                    return TryQuitDialogue();

                default:
                    return false;
            }
        }

        #endregion Overriden Methods

        #region Private Methods

        private void SetItemToInspect()
        {
            if (!StateData.Transform.TryGetComponent(out _data.ItemToInspect)) return;

            _data.Prefab = _data.ItemToInspect.GetItemPrefab();
            _data.ItemToInspect.PrepareInspection();
        }

        private void EnterDialogue()
        {
            if (_data.character && StateData.Transform.TryGetComponent(out _data.character))
            {
                _data.character.StartInteraction();
<<<<<<< Updated upstream
=======
                DialogueRunner().onDialogueComplete.AddListener(DialogueEnd);
                //DialogueRunner().StartDialogue("Phase_0",_data.character.CharacterName() , debug.ToString());
                debug++;
                // reenabling the input on the dialogue
                DialogueAdvanceInput().enabled = true;
>>>>>>> Stashed changes
            }
            if (StateData.Transform.TryGetComponent(out _data.ItemToInputInto))
            {
                //_data.ItemToInputInto.StartInteraction();
            }
        }

        public void HandleDialogue()
        { }

        //private void HandleDialogue() => DialogueManager.Instance.HandleDialogue();

        private bool TryQuitDialogue()
        {
            // if (!DialogueManager.Instance.CanQuitDialogue) return false;

            PopState();

            return true;
        }

        private void ActivateUI()
        {
            GameStateManager.Instance.ChangeState(GameState.UI);

            UIPanelManager.CloseOpenPanels();
            UIPanelManager.ToggleUIPanel(Panel);

            Player.InspectionLightSource.SetActive(true);
            Player.FirstPersonCamera.fieldOfView = _initialFOV;

            if (_data.ItemToInputInto)
            {
                UIPanelManager.EnableTextInput(TextInput);
            }
        }

        private void DeactivateUI()
        {
            GameStateManager.Instance.ChangeState(GameState.UI);

            UIPanelManager.ToggleUIPanel(Panel);

            Player.InspectionLightSource.SetActive(false);
            Player.FirstPersonCamera.fieldOfView = _initialFOV;
        }

        #endregion Private Methods
    }
}