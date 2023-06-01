using DonutDiner.InteractionModule.Environment;
using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;
using UnityEngine;

namespace DonutDiner.PlayerModule.States
{
    public class PlayerIdleState : PlayerBaseState
    {
        #region Fields

        [SerializeField] private float _gravityPull = 10.0f;

        #endregion Fields

        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            StateData = new PlayerStateData(dto);
        }

        public override PlayerActionDTO ExitState() => StateData.GetData();

        public override void UpdateState()
        {
            HandleGravity();
            CheckSwitchState();
        }

        public override void CheckSwitchState()
        {
            if (!Controller.isGrounded)
            {
                SwapState(StateMachine.Fall());
                return;
            }

            if (HasNoInput()) return;

            SwapState(StateMachine.Walk());
        }

        public override bool TrySwitchState(ActionType action, ActionDTO dto = null)
        {
            if (base.TrySwitchState(action, dto)) return false;

            switch (action)
            {
                case ActionType.Crouch:
                    SwapState(StateMachine.Crouch());
                    return true;

                case ActionType.Interact when dto is ItemSpotActionDTO actionDto:
                    return TryPlaceItem(actionDto.ItemSpot);

                case ActionType.Carry:
                    StateData.SetData(dto);
                    AppendState(StateMachine.Carry());
                    return true;

                case ActionType.Inspect when dto is TransformActionDTO:
                    StateData.SetData(dto);
                    PushState(StateMachine.Inspect());
                    return true;

                case ActionType.Interact when dto is InteractiveActionDTO actionDto:
                    if (actionDto.Interactive.IsInteractable())
                    { actionDto.Interactive.StartInteraction(); }
                    return false;

                case ActionType.Interact when dto is ItemSpotActionDTO actionDto:
                    if (!actionDto.ItemSpot.Item || actionDto.ItemSpot.IsItemLocked) return false;

                    StateData.SetData(new TransformActionDTO(actionDto.ItemSpot.Item));
                    AppendState(StateMachine.Carry());
                    return true;

                //case ActionType.Dialogue:
                //    StateData.SetData(dto);
                //    PushState(StateMachine.Dialogue());
                //    return true;

                default:
                    return false;
            }
        }

        #endregion Overriden Methods

        #region Private Methods

        private void HandleGravity()
        {
            Vector3 gravityVector = _gravityPull * Vector3.down;

            Controller.Move(gravityVector * Time.fixedDeltaTime);
        }

        private bool TryPlaceItem(ItemSpot itemSpot)
        {
            if (!itemSpot.TryPlaceItem(StateData.Transform)) return false;

            RemoveState();

            return true;
        }

        private bool HasNoInput() => PlayerInput.MovementInputValues.y == 0.0f && PlayerInput.MovementInputValues.x == 0.0f;

        #endregion Private Methods
    }
}