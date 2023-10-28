using Event.Signal;
using Status;
using UI.Game;
using UniRx;
using UnityEngine;

namespace Event
{
    public class Stage3BossDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            MessageBroker.Default.Publish(new EnemyBreakdownSignal(GameState.Stage3));
            MessageBroker.Default.Publish(new GameClear(TalkEnum.TalkStart));
        }
    }
}
