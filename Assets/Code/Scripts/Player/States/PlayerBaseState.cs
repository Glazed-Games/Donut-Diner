using DonutDiner.FrameworkModule;
using UnityEngine;

namespace DonutDiner.PlayerModule.States
{
    public abstract class PlayerBaseState : BaseState
    {
        #region Fields

        private readonly float _maxVerticalAngle = 90.0f;
        private readonly float _minVerticalAngle = -75.0f;
        private float _mouseX;
        private float _mouseY;
        private float _xAxisRotation;

        protected CharacterController Controller;

        private PlayerController _context;

        #endregion

        #region Public Methods

        public virtual void UpdateCamera()
        {
            _mouseX = PlayerInput.ViewInputValues.x * GameOptions.MouseSensitivity * Time.deltaTime;
            _mouseY = PlayerInput.ViewInputValues.y * GameOptions.MouseSensitivity * Time.deltaTime;

            Player.Sight.Rotate(Vector3.left * _mouseY);
            Player.Transform.Rotate(Vector3.up * _mouseX);

            ClampViewAngle();
        }

        #endregion

        #region Protected Methods

        protected override void SwitchState(BaseState newState)
        {
            base.SwitchState(newState);

            _context.SetCurrentState(newState as PlayerBaseState);
        }

        protected virtual void ClampViewAngle()
        {
            _xAxisRotation += _mouseY;

            if (_xAxisRotation > _maxVerticalAngle)
            {
                _xAxisRotation = _maxVerticalAngle;
                _mouseY = 0.0f;
                Player.Sight.ClampRotation(-_maxVerticalAngle);
            }
            else if (_xAxisRotation < _minVerticalAngle)
            {
                _xAxisRotation = _minVerticalAngle;
                _mouseY = 0.0f;
                Player.Sight.ClampRotation(-_minVerticalAngle);
            }
        }

        protected override void SetReferences()
        {
            base.SetReferences();

            Controller = GetComponentInParent<CharacterController>();
            _context = GetComponentInParent<PlayerController>();
        }

        #endregion
    }
}