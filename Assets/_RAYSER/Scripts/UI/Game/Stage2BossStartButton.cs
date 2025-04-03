using Event.Signal;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class Stage2BossStartButton : MonoBehaviour
    {
        public void PushButton()
        {
            MessageBroker.Default.Publish(new Stage2BossEncounter(TalkEnum.TalkStart));
        }
    }
}
