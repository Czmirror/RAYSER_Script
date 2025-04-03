using UI.Game;

namespace _RAYSER.Scripts.Event.Signal
{
    public class TutorialEnd
    {
        public readonly TalkEnum _talk;

        public TutorialEnd(TalkEnum talkEnum)
        {
            _talk = talkEnum;
        }
    }
}
