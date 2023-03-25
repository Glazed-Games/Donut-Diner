using System;

namespace DonutDiner.FrameworkModule
{
    public class GameStateManager : ComponentSingleton<GameStateManager>
    {
        #region Delegates

        public event Action<GameState> OnGameStateChanged = delegate { };

        #endregion

        #region Properties

        public GameState CurrentGameState { get; private set; }
        public GameState? PreviousGameState { get; private set; }

        #endregion

        #region Public Methods

        public void ChangeState(GameState gameState)
        {
            if (gameState == GameState.Gameplay && gameState == CurrentGameState) return;

            GameState newGameState = GetNewGameState(gameState);

            CurrentGameState = newGameState;
            OnGameStateChanged(newGameState);
        }

        #endregion

        #region Protected Methods

        protected override void Initialize()
        {
            CurrentGameState = GameState.Gameplay;
        }

        #endregion

        #region Private Methods

        private GameState GetNewGameState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Paused when PreviousGameState != null:
                    gameState = (GameState)PreviousGameState;
                    PreviousGameState = null;
                    break;

                case GameState.Paused:
                    PreviousGameState = CurrentGameState;
                    break;

                default:
                    if (gameState == CurrentGameState)
                    {
                        gameState = GameState.Gameplay;
                    }

                    break;
            }

            return gameState;
        }

        #endregion
    }
}