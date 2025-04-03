using Event.Signal;
using UI.Game;
using UniRx;
using UnityEngine;

namespace EnemyMove
{
    public class Stage2BossActivate : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
            MessageBroker.Default.Receive<Stage2BossEncounter>().Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => gameObject.SetActive(true)).AddTo(this);
        }

    }
}
