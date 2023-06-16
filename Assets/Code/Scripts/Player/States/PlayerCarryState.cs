using DonutDiner.InteractionModule.Environment;
using DonutDiner.ItemModule.Items;
using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;

namespace DonutDiner.PlayerModule.States
{
    public class PlayerCarryState : BaseState
    {
        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            //if carrying and object already, drop it and then pick up the new item
            if (StateData != null && StateData.Transform != null)
            { DropItem(); }

            StateData = new PlayerStateData(dto);
            CarryItem();
        }

        public override PlayerActionDTO ExitState() => null;

        public override void UpdateState()
        {
            CheckSwitchState();
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
                    if (StateData != null && StateData.Transform != null)
                    { DropItem(); }
                    RemoveState();
                    return true;

                case ActionType.Carry:
                    if (StateData != null && StateData.Transform != null)
                    { DropItem(); }
                    return false;

                case ActionType.Inspect when StateData.Transform.TryGetComponent(out ItemToInspect _):
                    SuperState.PushState(StateMachine.Inspect(), new TransformActionDTO(StateData.Transform));
                    return true;

                case ActionType.Interact when dto is ItemSpotActionDTO actionDto:
                    return TryPlaceItem(actionDto.ItemSpot);

                case ActionType.Inventory:
                    // PopState();
                    //StateData.SetData(dto);
                    AppendState(StateMachine.Menu());
                    // SuperState.PushState(StateMachine.Menu(), dto);

                    return true;

                default:
                    return false;
            }
        }

        #endregion Overriden Methods

        #region Private Methods

        private void CarryItem()
        {
            if (StateData.Transform.TryGetComponent(out ItemToCarry item))
            {
                item.Carry();
            }
        }

        private void DropItem()
        {
            if (StateData.Transform.TryGetComponent(out ItemToCarry item))
            {
                item.Drop();
            }
        }

        private bool TryPlaceItem(ItemSpot itemSpot)
        {
            if (!itemSpot.TryPlaceItem(StateData.Transform)) return false;

            RemoveState();

            return true;
        }

        #endregion Private Methods
    }
}