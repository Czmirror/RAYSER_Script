using UI.Game;

namespace Event.Signal
{
    public class Stage1BossEncounter
    {
        /// <summary>
        /// 会話
        /// </summary>
        public readonly TalkEnum _talk;

        /// <summary>
        /// 会話設定
        /// </summary>
        /// <param name="talkEnum">会話Enum</param>
        public Stage1BossEncounter(TalkEnum talkEnum)
        {
            _talk = talkEnum;
        }
    }
}
