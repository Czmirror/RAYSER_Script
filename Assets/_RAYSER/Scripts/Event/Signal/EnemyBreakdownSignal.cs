using Status;

namespace Event.Signal
{
    /// <summary>
    /// 敵機全滅処理通知
    /// </summary>
    public class EnemyBreakdownSignal
    {
        /// <summary>
        /// 全滅対象となるゲームステート
        /// </summary>
        public GameState SettingGameState;

        public EnemyBreakdownSignal(GameState gameState)
        {
            SettingGameState = gameState;
        }
    }
}
