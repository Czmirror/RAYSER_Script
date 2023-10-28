using Event.Signal;
using Status;
using UI.Game;
using UniRx;
using UnityEngine;

namespace Event
{
    public class Stage2BossDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            MessageBroker.Default.Publish(new EnemyBreakdownSignal(GameState.Stage2));
            MessageBroker.Default.Publish(new EnemyBreakdownSignal(GameState.Stage2Boss));
            MessageBroker.Default.Publish(new Stage3IntervalStart(TalkEnum.TalkStart));
        }
    }
}
