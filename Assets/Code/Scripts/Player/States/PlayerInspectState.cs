using DonutDiner.FrameworkModule;
using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;
using DonutDiner.UIModule;
using UnityEngine;

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

        private InspectStateData _data;

        #endregion

        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            StateData = new InspectStateData(dto);

            _data = StateData as InspectStateData;

            ActivateUI();
            SetItemToInspect();
        }

        public override PlayerActionDTO ExitState()
        {
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
                    PopState();
                    return true;

                default:
                    return false;
            }
        }

        #endregion

        #region Private Methods

        private void HandleRotation()
        {
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

            UIPanelManager.ToggleUIPanel(Panel);

            Player.InspectionLightSource.SetActive(false);
            Player.FirstPersonCamera.fieldOfView = _initialFOV;
        }

        private void SetItemToInspect()
        {
            if (!StateData.Transform.TryGetComponent(out _data.ItemToInspect)) return;

            _data.Prefab = _data.ItemToInspect.GetItemPrefab();
            _data.ItemToInspect.PrepareInspection();
        }

        #endregion
    }
}