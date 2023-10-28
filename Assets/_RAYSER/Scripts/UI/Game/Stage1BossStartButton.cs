using Event;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class Stage1BossStartButton : MonoBehaviour
    {
        public void PushButton()
        {
            MessageBroker.Default.Publish(new Stage1BossEncounter(TalkEnum.TalkStart));
        }
    }
}
