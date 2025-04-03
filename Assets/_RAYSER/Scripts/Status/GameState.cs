namespace Status
{
    /// <summary>
    /// ゲーム状態
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// ゲームスタート
        /// </summary>
        Gamestart,

        /// <summary>
        /// ステージ１
        /// </summary>
        Stage1,

        /// <summary>
        /// ステージ１ボス
        /// </summary>
        Stage1Boss,

        /// <summary>
        /// ステージ２への移動
        /// </summary>
        Stage2Interval,

        /// <summary>
        /// ステージ２
        /// </summary>
        Stage2,

        /// <summary>
        /// ステージ２ボス
        /// </summary>
        Stage2Boss,

        /// <summary>
        /// ステージ３への移動
        /// </summary>
        Stage3Interval,

        /// <summary>
        /// ステージ３
        /// </summary>
        Stage3,

        /// <summary>
        /// ゲームクリア
        /// </summary>
        GameClear,

        /// <summary>
        /// ゲームオーバー
        /// </summary>
        Gameover,

        /// <summary>
        /// チュートリアル
        /// </summary>
        Tutorial
    }
}
