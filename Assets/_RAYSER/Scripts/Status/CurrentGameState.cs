namespace Status
{
    /// <summary>
    /// 現在のゲームステートを管理するクラス、用途コンティニュー時のステート管理など
    /// </summary>
    public class CurrentGameState
    {
        private GameState _getState;


        public GameState GetState
        {
            get { return _getState; }
        }

        public void SetGameStatus(GameState gameState)
        {
            _getState = gameState;
        }

        public void Reset()
        {
            _getState = GameState.Stage1;
        }
    }
}
