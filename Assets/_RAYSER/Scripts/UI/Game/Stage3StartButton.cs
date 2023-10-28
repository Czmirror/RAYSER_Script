using Event;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class Stage3StartButton : MonoBehaviour
    {
        public void PushButton()
        {
            MessageBroker.Default.Publish(new Stage3IntervalStart(TalkEnum.TalkStart));
        }
    }
}
