using VContainer;

namespace Status
{
    public class GameStateService
    {
        private CurrentGameState currentGameState;

        [Inject]
        public void Construct(CurrentGameState currentGameState)
        {
            this.currentGameState = currentGameState;
        }

        public GameState GetGameStatus()
        {
            return this.currentGameState.GetState;
        }

        public void SetGameStatus(GameState gameState)
        {
            currentGameState.SetGameStatus(gameState);
        }

        public void Reset()
        {
            currentGameState.SetGameStatus(GameState.Gamestart);
        }
    }
}
