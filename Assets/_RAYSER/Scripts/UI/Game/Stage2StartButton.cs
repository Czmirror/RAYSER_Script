using Event;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class Stage2StartButton : MonoBehaviour
    {
        public void PushButton()
        {
            MessageBroker.Default.Publish(new Stage2IntervalStart(TalkEnum.TalkStart));
        }
    }
}
