using DonutDiner.PlayerModule.States.Data;
using DonutDiner.PlayerModule.States.DTOs;
using UnityEngine;

namespace DonutDiner.PlayerModule.States
{
    public class PlayerFallState : PlayerBaseState
    {
        #region Fields

        [SerializeField] private AnimationCurve _gravityCurve;
        [SerializeField] private float _airborneSpeed = 2.0f;
        [SerializeField] private float _gravityPull = 18.0f;
        [SerializeField] private float _maxVelocity = 90.0f;
        [SerializeField] private float _coyoteTime = 0.2f;

        private FallStateData _data;

        #endregion

        #region Overriden Methods

        public override void EnterState(PlayerActionDTO dto)
        {
            StateData = new FallStateData(dto);

            _data = StateData as FallStateData;
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
            if (!Controller.isGrounded) return;

            if (HasNoInput())
            {
                SwapState(StateMachine.Idle());
                return;
            }

            SwapState(StateMachine.Walk());
        }

        public override bool TrySwitchState(ActionType action, ActionDTO dto = null)
        {
            if (base.TrySwitchState(action, dto)) return false;

            return false;
        }

        #endregion

        #region Private Methods

        private void HandleMovement()
        {
            Vector3 movementVector = HandleInput();
            Vector3 gravityVector = HandleGravity();

            SetTimer();
            SetMomentum();

            movementVector = gravityVector + _data.Momentum + movementVector * _airborneSpeed;

            Controller.Move(movementVector * Time.fixedDeltaTime);
        }

        private Vector3 HandleInput()
        {
            Vector3 movementVector = Player.Transform.forward * PlayerInput.MovementInputValues.y +
                                     Player.Transform.right * PlayerInput.MovementInputValues.x;

            return movementVector.normalized;
        }

        private Vector3 HandleGravity()
        {
            Vector3 gravityVector = _gravityCurve.Evaluate(_data.TimeFalling) * _gravityPull * Vector3.down;

            gravityVector.y = Mathf.Clamp(gravityVector.y, -_maxVelocity, 0.0f);

            return gravityVector;
        }

        private void SetTimer()
        {
            _data.TimeFalling += Time.fixedDeltaTime;
            _data.CoyoteTimeCounter += Time.fixedDeltaTime;
        }

        private void SetMomentum()
        {
            _data.Momentum = Vector3.Lerp(_data.Momentum, Vector3.zero, Time.fixedDeltaTime * 5.0f);
        }

        private bool HasNoInput() => PlayerInput.MovementInputValues.y == 0.0f && PlayerInput.MovementInputValues.x == 0.0f;

        #endregion
    }
}