using DonutDiner.FrameworkModule;
using DonutDiner.ItemModule.Items;
using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;
using DonutDiner.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace DonutDiner.PlayerModule.States
{
    public class PlayerInspectState : PlayerBaseState
    {
        #region Fields

        [SerializeField] private float _initialFOV = 60.0f;
        [SerializeField] private float _minZoom = 30.0f;
        [SerializeField] private float _maxZoom = 100.0f;
        [SerializeField] private float _zoomSpeed = 1000.0f;
        [SerializeField] private float _rotationSpeed = 5.0f;

        public static GameObject Panel;
        public static GameObject TextInput;
        public static GameObject DonutBoxPanel;
        public static GameObject JournalPanel;

        private InspectStateData _data;

        #endregion Fields

        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            StateData = new InspectStateData(dto);

            _data = StateData as InspectStateData;

            ActivateUI();
            SetItemToInspect();

            if (_data.ItemToInspect.gameObject.GetComponent<ItemToInputInto>())
            {
                UIPanelManager.EnableTextInput(TextInput);
            }
        }

        public override PlayerActionDTO ExitState()
        {
            if (_data.ItemToInspect.GetComponent<ItemToInputInto>())
            {
                if (TextInput.GetComponent<InputField>())
                { _data.ItemToInspect.GetComponent<ItemToInputInto>().GetTextInput(TextInput.GetComponent<InputField>().text); }
            }

            DeactivateUI();

            if (_data.ItemToInspect) _data.ItemToInspect.FinishInspection();

            return StateData.GetData();
        }

        public override void UpdateStates()
        {
            UpdateState();
        }

        public override void UpdateState()
        {
            HandleRotation();
        }

        public override void UpdateCamera()
        {
            HandleZoom();
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

                case ActionType.Carry:

                    PopState();
                    return true;

                case ActionType.Inspect:

                    if (_data.ItemToInspect.GetComponent<ItemToInputInto>())
                    {
                        //If this has an input field associated with it ignore the interact/inspect key
                        return false;
                    }
                    PopState();
                    return true;

                default:
                    return false;
            }
        }

        public override void TryHandleUseItem(Transform item)
        {
            if (item == null)
            { return; }

            //_data.ItemToInspect
        }

        #endregion Overriden Methods

        #region Private Methods

        private void HandleRotation()
        {
            if (_data.ItemToInspect.gameObject.GetComponent<ItemToInputInto>())
            {
                return;
            }
            float inputX = PlayerInput.RotationInputValues.x * _rotationSpeed;
            float inputY = PlayerInput.RotationInputValues.y * _rotationSpeed;

            _data.Prefab.transform.Rotate(Vector3.back, -inputY, Space.World);
            _data.Prefab.transform.Rotate(Vector3.up, -inputX, Space.World);
        }

        private void HandleZoom()
        {
            if (PlayerInput.ZoomInputValues.y > 0)
            {
                Player.FirstPersonCamera.fieldOfView -= _zoomSpeed * Time.deltaTime;
            }
            else if (PlayerInput.ZoomInputValues.y < 0)
            {
                Player.FirstPersonCamera.fieldOfView += _zoomSpeed * Time.deltaTime;
            }

            Player.FirstPersonCamera.fieldOfView = Mathf.Clamp(Player.FirstPersonCamera.fieldOfView, _minZoom, _maxZoom);
        }

        private void ActivateUI()
        {
            GameStateManager.Instance.ChangeState(GameState.UI);

            UIPanelManager.CloseOpenPanels();
            UIPanelManager.ToggleUIPanel(Panel);

            Player.InspectionLightSource.SetActive(true);
            Player.FirstPersonCamera.fieldOfView = _initialFOV;
        }

        private void DeactivateUI()
        {
            GameStateManager.Instance.ChangeState(GameState.UI);

            //UIPanelManager.ToggleUIPanel(Panel);
            //UIPanelManager.ToggleUIPanel(TextInput);
            UIPanelManager.CloseAllPanels();

            Player.InspectionLightSource.SetActive(false);
            Player.FirstPersonCamera.fieldOfView = _initialFOV;
        }

        private void SetItemToInspect()
        {
            if (!StateData.Transform.TryGetComponent(out _data.ItemToInspect)) return;

            _data.Prefab = _data.ItemToInspect.GetItemPrefab();
            _data.ItemToInspect.PrepareInspection();
        }

        #endregion Private Methods
    }
}