using DonutDiner.FrameworkModule;
using DonutDiner.InteractionModule.Environment;
using DonutDiner.InteractionModule.Interactive;
using DonutDiner.ItemModule.Items;
using DonutDiner.PlayerModule.States.DTOs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DonutDiner.PlayerModule
{
    public class PlayerInput : MonoBehaviour
    {
        #region Fields

        private static Vector2 _movementInputValues;
        private static Vector2 _viewInputValues;
        private static Vector2 _cursorInputValues;
        private static Vector2 _zoomInputValues;
        private static Vector2 _rotationInputValues;

        private bool _isGameplayEnabled;
        private bool _isUIEnabled;

        private InputActions _actions;
        private PlayerController _context;
        private PlayerInteraction _interaction;

        #endregion

        #region Properties

        public static Vector2 MovementInputValues => _movementInputValues;
        public static Vector2 ViewInputValues => _viewInputValues;
        public static Vector2 CursorInputValues => _cursorInputValues;
        public static Vector2 ZoomInputValues => _zoomInputValues;
        public static Vector2 RotationInputValues => _rotationInputValues;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            SetReferences();
        }

        private void OnEnable()
        {
            EnableInputActions();
            SubscribeEvents();
        }

        private void Start()
        {
            _isGameplayEnabled = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        #region Public Methods

        public void UpdateInput()
        {
            if (_isGameplayEnabled) SetGameplayInput();
            else if (_isUIEnabled) SetUIInput();
        }

        #endregion

        #region Private Methods

        #region Input Actions

        private void OnCrouchPressed(InputAction.CallbackContext callback)
        {
            _context.CurrentState.TrySwitchState(ActionType.Crouch);
        }

        private void OnInteractionPressed(InputAction.CallbackContext callback)
        {
            if (!_interaction.Interaction)
            {
                _context.CurrentState.TrySwitchState(ActionType.None);

                return;
            }

            Transform interaction = _interaction.Interaction;

            if (TryHandleInteractive(interaction)) return;

            if (TryHandleItem(interaction)) return;

            HandleInteraction(interaction);
        }

        private void OnInspectionPressed(InputAction.CallbackContext callback)
        {
            _context.CurrentState.TrySwitchState(ActionType.Inspect);
        }

        private void OnInventoryPressed(InputAction.CallbackContext callback)
        {
            _context.CurrentState.TrySwitchState(ActionType.Inventory);
        }

        private void OnPausePressed(InputAction.CallbackContext callback)
        {
            GameStateManager.Instance.ChangeState(GameState.Paused);
        }

        private void OnEscapePressed(InputAction.CallbackContext callback)
        {
            if (_isUIEnabled)
            {
                _context.CurrentState.TrySwitchState(ActionType.None);

                return;
            }

            GameStateManager.Instance.ChangeState(GameState.Paused);
            // TOGGLE GAME MENU
        }

        #endregion

        private bool TryHandleInteractive(Transform interaction)
        {
            if (!_interaction.TryGetInteractive(out IInteractive interactive)) return false;

            switch (interactive)
            {
                case NPC:
                    _context.CurrentState.TrySwitchState(ActionType.Dialogue, new TransformActionDTO(interaction));
                    break;

                default:
                    _context.CurrentState.TrySwitchState(ActionType.Interact, new InteractiveActionDTO(interactive));
                    break;
            }

            return true;
        }

        private bool TryHandleItem(Transform interaction)
        {
            if (!_interaction.TryGetItem(out IItem item)) return false;

            switch (item)
            {
                case ItemToPickUp:
                    // HANDLE PICKING UP
                    break;

                case ItemToCarry:
                    _context.CurrentState.TrySwitchState(ActionType.Carry, new TransformActionDTO(interaction));
                    break;

                case ItemToInspect when interaction.TryGetComponent(out ItemToCarry _):
                    _context.CurrentState.TrySwitchState(ActionType.Carry, new TransformActionDTO(interaction));
                    break;

                case ItemToInspect:
                    _context.CurrentState.TrySwitchState(ActionType.Inspect, new TransformActionDTO(interaction));
                    break;

                default:
                    break;
            }

            return true;
        }

        private void HandleInteraction(Transform interaction)
        {
            switch (LayerMask.LayerToName(interaction.gameObject.layer))
            {
                case Layer.ToInteract when interaction.TryGetComponent(out ItemSpot itemSpot):
                    _context.CurrentState.TrySwitchState(ActionType.Interact, new ItemSpotActionDTO(itemSpot));
                    break;

                case Layer.ToInspect:
                    _context.CurrentState.TrySwitchState(ActionType.None);
                    break;

                default:
                    _context.CurrentState.TrySwitchState(ActionType.None);
                    break;
            }
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Gameplay:
                    EnableInputActions();
                    ToggleInputReading(true, false);
                    break;

                case GameState.Paused:
                    DisableInputActions();
                    ToggleInputReading(false, false);
                    break;

                case GameState.UI:
                    EnableInputActions();
                    ToggleInputReading(false, true);
                    break;

                default:
                    break;
            }
        }

        private void SetGameplayInput()
        {
            _movementInputValues = _actions.Player.Movement.ReadValue<Vector2>();
            _viewInputValues = _actions.Player.View.ReadValue<Vector2>();
        }

        private void SetUIInput()
        {
            _cursorInputValues = _actions.UI.Cursor.ReadValue<Vector2>();
            _zoomInputValues = _actions.UI.Zoom.ReadValue<Vector2>();

            if (_actions.UI.Hold.IsPressed())
            {
                _rotationInputValues = _actions.UI.Rotate.ReadValue<Vector2>();
            }
            else if (_actions.UI.Hold.WasReleasedThisFrame())
            {
                _rotationInputValues = Vector3.zero;
            }
        }

        private void ToggleInputReading(bool isGameplayEnabled, bool isUIEnabled)
        {
            _isGameplayEnabled = isGameplayEnabled;
            _isUIEnabled = isUIEnabled;
        }

        private void EnableInputActions()
        {
            if (!_actions.Game.enabled) _actions.Game.Enable();
            if (!_actions.Player.enabled) _actions.Player.Enable();
            if (!_actions.UI.enabled) _actions.UI.Enable();
        }

        private void DisableInputActions()
        {
            _actions.Player.Disable();
            _actions.UI.Disable();
        }

        private void SubscribeEvents()
        {
            _actions.Player.Crouch.performed += OnCrouchPressed;
            _actions.Player.Interact.performed += OnInteractionPressed;
            _actions.Player.Inspect.performed += OnInspectionPressed;
            _actions.UI.Inventory.performed += OnInventoryPressed;
            _actions.Game.Pause.performed += OnPausePressed;
            _actions.Game.Escape.performed += OnEscapePressed;

            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void UnsubscribeEvents()
        {
            _actions.Player.Crouch.performed -= OnCrouchPressed;
            _actions.Player.Interact.performed -= OnInteractionPressed;
            _actions.Player.Inspect.performed -= OnInspectionPressed;
            _actions.UI.Inventory.performed -= OnInventoryPressed;
            _actions.Game.Pause.performed -= OnPausePressed;
            _actions.Game.Escape.performed -= OnEscapePressed;

            if (GameStateManager.Instance != null) GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        }

        private void SetReferences()
        {
            _actions = new InputActions();
            _context = GetComponent<PlayerController>();
            _interaction = GetComponent<PlayerInteraction>();
        }

        #endregion
    }
}