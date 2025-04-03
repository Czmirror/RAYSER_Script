using UI.Game;

namespace _RAYSER.Scripts.Event.Signal
{
    public class TutorialStart
    {
        public readonly TalkEnum _talk;

        public TutorialStart(TalkEnum talkEnum)
        {
            _talk = talkEnum;
        }
    }
}
