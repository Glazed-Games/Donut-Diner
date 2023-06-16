using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;
using UnityEngine;

namespace DonutDiner.PlayerModule.States
{
    public class PlayerWalkState : PlayerBaseState
    {
        #region Fields

        [SerializeField] private float _speed = 10.0f;
        [SerializeField] private float _acceleration = 0.0f;
        [SerializeField] private float _gravityPull = 10.0f;

        #endregion Fields

        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            StateData = new PlayerStateData(dto)
            {
                Speed = _speed
            };
        }

        public override PlayerActionDTO ExitState()
        {
            StateData.SetData(new MovementActionDTO(Controller.velocity));

            return StateData.GetData();
        }

        public override void UpdateState()
        {
            HandleMovement();
            CheckSwitchState();
        }

        public override void CheckSwitchState()
        {
            if (!Controller.isGrounded)
            {
                SwapState(StateMachine.Fall());
                return;
            }

            if (HasNoInput())
            {
                SwapState(StateMachine.Idle());
            }
        }

        public override bool TrySwitchState(ActionType action, ActionDTO dto = null)
        {
            if (base.TrySwitchState(action, dto)) return false;

            switch (action)
            {
                case ActionType.Crouch:
                    SwapState(StateMachine.Crouch());
                    return true;

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

                case ActionType.Inventory:
                    StateData.SetData(dto);
                    AppendState(StateMachine.Menu());
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

        private void HandleMovement()
        {
            Vector3 movementVector = HandleInput();

            movementVector = movementVector * StateData.Speed + _gravityPull * Vector3.down;

            Controller.Move(movementVector * Time.smoothDeltaTime);
        }

        private Vector3 HandleInput()
        {
            Vector3 movementVector = Player.Transform.forward * PlayerInput.MovementInputValues.y +
                                     Player.Transform.right * PlayerInput.MovementInputValues.x;

            return movementVector.normalized;
        }

        private bool HasNoInput() => PlayerInput.MovementInputValues.y == 0.0f && PlayerInput.MovementInputValues.x == 0.0f;

        #endregion Private Methods
    }
}