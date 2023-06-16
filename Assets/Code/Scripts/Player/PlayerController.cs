using DonutDiner.FrameworkModule;
using DonutDiner.PlayerModule.States;
using DonutDiner.PlayerModule.States.DTOs;
using UnityEngine;

namespace DonutDiner.PlayerModule
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields

        private PlayerStateMachine _stateMachine;
        private PlayerInput _playerInput;
        private PlayerInteraction _interaction;

        #endregion Fields

        #region Properties

        public bool IsPaused { get; private set; }
        public PlayerBaseState CurrentState { get; private set; }
        public PlayerBaseState debugShowState;

        #endregion Properties

        #region Unity Methods

        private void Awake()
        {
            SetReferences();
        }

        private void OnEnable()
        {
            GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void Start()
        {
            CurrentState = _stateMachine.Idle();
            CurrentState.EnterState(new PlayerActionDTO(0.0f, false, Vector3.zero));
        }

        private void Update()
        {
            _playerInput.UpdateInput();
        }

        private void FixedUpdate()
        {
            if (IsPaused) return;

            _interaction.Examine();

            CurrentState.UpdateStates();
        }

        private void LateUpdate()
        {
            debugShowState = CurrentState;
            if (IsPaused) return;

            CurrentState.UpdateCamera();
        }

        private void OnDisable()
        {
            if (GameStateManager.Instance != null)
            {
                GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            }
        }

        #endregion Unity Methods

        #region Public Methods

        public void SetCurrentState(PlayerBaseState newState)
        {
            CurrentState = newState;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnGameStateChanged(GameState gameState)
        {
            IsPaused = gameState == GameState.Paused;
        }

        private void SetReferences()
        {
            _stateMachine = GetComponentInChildren<PlayerStateMachine>();
            _playerInput = GetComponent<PlayerInput>();
            _interaction = GetComponent<PlayerInteraction>();
        }

        #endregion Private Methods
    }
}