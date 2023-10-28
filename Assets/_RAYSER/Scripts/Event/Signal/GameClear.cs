using UI.Game;

namespace Event.Signal
{
    /// <summary>
    /// ゲームクリア通知処理
    /// </summary>
    public class GameClear
    {
        /// <summary>
        /// 会話
        /// </summary>
        public readonly TalkEnum _talk;

        /// <summary>
        /// 会話設定
        /// </summary>
        /// <param name="talkEnum">会話Enum</param>
        public GameClear(TalkEnum talkEnum)
        {
            _talk = talkEnum;
        }
    }
}
