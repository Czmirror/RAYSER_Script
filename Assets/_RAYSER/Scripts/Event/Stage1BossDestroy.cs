using Event.Signal;
using Status;
using UI.Game;
using UniRx;
using UnityEngine;

namespace Event
{
    public class Stage1BossDestroy : MonoBehaviour
    {
        private void OnDestroy()
        {
            MessageBroker.Default.Publish(new EnemyBreakdownSignal(GameState.Stage1));
            MessageBroker.Default.Publish(new Stage2IntervalStart(TalkEnum.TalkStart));
        }
    }
}
