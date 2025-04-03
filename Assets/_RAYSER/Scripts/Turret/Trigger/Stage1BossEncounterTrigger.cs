using System;
using Event.Signal;
using Turret;
using UI.Game;
using UniRx;

namespace _RAYSER.Scripts.Turret.Trigger
{
    /// <summary>
    /// ステージ1ボスエンカウントトリガー
    /// </summary>
    [Serializable]
    public class Stage1BossEncounterTrigger : ITurretTrigger
    {
        public void Initialize(EnemyTurret turret)
        {
            MessageBroker.Default.Receive<Stage1BossEncounter>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => turret.StartShootingAsync())
                .AddTo(turret);
        }
    }
}
