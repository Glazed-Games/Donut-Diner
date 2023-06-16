using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;
using DonutDiner.UIModule;
using UnityEngine;

namespace DonutDiner.PlayerModule.States
{
    public class PlayerMenuState : PlayerBaseState
    {
        #region Fields

        [SerializeField] private float _initialFOV = 60.0f;

        public static GameObject Panel;

        #endregion Fields

        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            StateData = new PlayerStateData(dto);

            ActivateUI();
        }

        public override PlayerActionDTO ExitState()
        {
            DeactivateUI();

            return StateData.GetData();
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
            if (base.TrySwitchState(action, dto)) return false;

            switch (action)
            {
                case ActionType.None:
                    DeactivateUI();
                    RemoveState();
                    return true;

                case ActionType.Carry:
                    DeactivateUI();
                    RemoveState();
                    return false;

                case ActionType.Inventory:
                    DeactivateUI();
                    RemoveState();

                    return true;

                default:
                    return false;
            }
        }

        #endregion Overriden Methods

        #region Private Methods

        private void ActivateUI()
        {
            UIPanelManager.CloseOpenPanels();
            UIPanelManager.ToggleUIPanel(Panel);
        }

        private void DeactivateUI()
        {
            UIPanelManager.CloseAllPanels();
            Player.InspectionLightSource.SetActive(false);
            Player.FirstPersonCamera.fieldOfView = _initialFOV;
        }

        #endregion Private Methods
    }
}