using DonutDiner.PlayerModule.States;
using UnityEngine;

namespace DonutDiner.PlayerModule
{
    public class PlayerStateMachine : MonoBehaviour
    {
        #region Fields

        private PlayerIdleState _idleState;
        private PlayerWalkState _walkState;
        private PlayerCrouchState _crouchState;
        private PlayerFallState _fallState;
        private PlayerCarryState _carryState;
        private PlayerInspectState _inspectState;
        private PlayerDialogueState _dialogueState;
        private PlayerMenuState _menuState;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            TryGetComponent(out _idleState);
            TryGetComponent(out _walkState);
            TryGetComponent(out _crouchState);
            TryGetComponent(out _fallState);
            TryGetComponent(out _carryState);
            TryGetComponent(out _inspectState);
            TryGetComponent(out _dialogueState);
            TryGetComponent(out _dialogueState);
            TryGetComponent(out _menuState);
        }

        #endregion

        #region Public Methods

        public PlayerBaseState Idle()
        {
            return _idleState;
        }

        public PlayerBaseState Walk()
        {
            return _walkState;
        }

        public PlayerBaseState Crouch()
        {
            return _crouchState;
        }

        public PlayerBaseState Fall()
        {
            return _fallState;
        }

        public PlayerBaseState Dialogue()
        {
            return _dialogueState;
        }

        public BaseState Carry()
        {
            return _carryState;
        }

        public PlayerBaseState Inspect()
        {
            return _inspectState;
        }

        public PlayerBaseState Menu()
        {
            return _menuState;
        }

        //public PlayerBaseState Dialogue()
        //{
        //    return _dialogueState;
        //}

        #endregion
    }
}