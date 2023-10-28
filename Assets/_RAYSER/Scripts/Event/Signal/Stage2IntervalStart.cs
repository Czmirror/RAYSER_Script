using UI.Game;

namespace Event.Signal
{
    public class Stage2IntervalStart
    {
        /// <summary>
        /// 会話
        /// </summary>
        public readonly TalkEnum _talk;

        /// <summary>
        /// 会話設定
        /// </summary>
        /// <param name="talkEnum">会話Enum</param>
        public Stage2IntervalStart(TalkEnum talkEnum)
        {
            _talk = talkEnum;
        }
    }
}
