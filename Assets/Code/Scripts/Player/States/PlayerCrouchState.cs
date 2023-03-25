using DonutDiner.ItemModule.Items;
using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;
using System.Collections;
using UnityEngine;

namespace DonutDiner.PlayerModule.States
{
    public class PlayerCrouchState : PlayerBaseState
    {
        #region Fields

        [SerializeField] private float _speed = 5.0f;
        [SerializeField] private float _gravityPull = 10.0f;
        [SerializeField] private float _standingHeight = 2.0f;
        [SerializeField] private float _crouchingHeight = 1.0f;
        [SerializeField] private float _sightStandingHeight = 0.9f;
        [SerializeField] private float _sightCrouchingHeight = 0.3f;
        [SerializeField] private float _distanceToCeiling = 1.0f;
        [SerializeField] private float _interpolationRatio = 0.2f;
        [SerializeField] private float _smoothTime = 0.12f;

        private CrouchStateData _data;

        #endregion

        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            StateData = new CrouchStateData(new Vector3(0.0f, _sightStandingHeight), dto)
            {
                Speed = _speed,
                IsCrouching = true
            };

            _data = StateData as CrouchStateData;

            if (dto.IsCrouching) return;

            _data.Coroutine = StartCoroutine(TranslateHeight(_crouchingHeight,
                                                             GetTargetCenter(),
                                                             new Vector3(0.0f, _sightCrouchingHeight)));
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
            if (Controller.isGrounded) return;

            StandUpBeforeSwap(StateMachine.Fall(), true);
        }

        public override bool TrySwitchState(ActionType action, ActionDTO dto = null)
        {
            if (base.TrySwitchState(action, dto)) return false;

            switch (action)
            {
                case ActionType.Crouch when CanStandUp():
                    StandUpBeforeSwap(StateMachine.Idle());
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
                    actionDto.Interactive.StartInteraction();
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

        #endregion

        #region Private Methods

        private void HandleMovement()
        {
            Vector3 movementVector = HandleInput();

            movementVector = movementVector * _speed + _gravityPull * Vector3.down;

            Controller.Move(movementVector * Time.fixedDeltaTime);
        }

        private Vector3 HandleInput()
        {
            Vector3 movementVector = Player.Transform.forward * PlayerInput.MovementInputValues.y +
                                     Player.Transform.right * PlayerInput.MovementInputValues.x;

            return movementVector.normalized;
        }

        private bool CanStandUp() => !Physics.Raycast(Player.Transform.position, Player.Transform.up, _distanceToCeiling);

        private void StandUpBeforeSwap(PlayerBaseState newState, bool skipTransition = false)
        {
            if (_data.Coroutine != null) StopCoroutine(_data.Coroutine);

            _data.IsCrouching = false;

            if (skipTransition)
            {
                SetFinalPosition(_standingHeight, Vector3.zero, _data.SightPosition);

                SwapState(newState);

                return;
            }

            _data.Coroutine = StartCoroutine(TranslateHeight(_standingHeight,
                                                             Vector3.zero,
                                                             _data.SightPosition,
                                                             newState));
        }

        private Vector3 GetTargetCenter() => -new Vector3(0.0f, 1 - _crouchingHeight / Controller.height, 0.0f);

        private void SetFinalPosition(float targetHeight, Vector3 targetCenter, Vector3 targetPosition)
        {
            Controller.height = targetHeight;
            Controller.center = targetCenter;
            Player.Sight.localPosition = targetPosition;
        }

        #region Coroutine

        private IEnumerator TranslateHeight(float targetHeight, Vector3 targetCenter, Vector3 targetPosition)
        {
            Vector3 velocityVector = Vector3.zero;

            while (IsFarFromTarget(targetHeight, targetPosition.y))
            {
                Controller.height = Mathf.LerpUnclamped(Controller.height, targetHeight, _interpolationRatio);
                Controller.center = Vector3.LerpUnclamped(Controller.center, targetCenter, _interpolationRatio);

                velocityVector = MoveCameraTowards(targetPosition, velocityVector);

                yield return new WaitForFixedUpdate();
            }

            SetFinalPosition(targetHeight, targetCenter, targetPosition);

            _data.Coroutine = null;

            yield return null;
        }

        private IEnumerator TranslateHeight(float targetHeight,
                                            Vector3 targetCenter,
                                            Vector3 targetPosition,
                                            PlayerBaseState newState)
        {
            yield return TranslateHeight(targetHeight, targetCenter, targetPosition);

            SwapState(newState);
        }

        private bool IsFarFromTarget(float targetHeight, float targetYPosition)
        {
            return Mathf.Abs(Player.Sight.position.y - targetYPosition) >= 0.01f &&
                   Mathf.Abs(Controller.height - targetHeight) >= 0.01f;
        }

        private Vector3 MoveCameraTowards(Vector3 targetPosition, Vector3 velocityVector)
        {
            Player.Sight.localPosition = Vector3.SmoothDamp(Player.Sight.localPosition,
                                                            targetPosition,
                                                            ref velocityVector,
                                                            _smoothTime);

            return velocityVector;
        }

        #endregion

        #endregion
    }
}