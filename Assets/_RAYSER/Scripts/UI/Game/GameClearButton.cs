using Event;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class GameClearButton : MonoBehaviour
    {
        public void PushButton()
        {
            MessageBroker.Default.Publish(new GameClear(TalkEnum.TalkStart));
        }
    }
}
