using System;
using Event.Signal;
using Turret;
using UI.Game;
using UniRx;

namespace _RAYSER.Scripts.Turret.Trigger
{
    /// <summary>
    /// ステージ2ボスエンカウントトリガー
    /// </summary>
    [Serializable]
    public class Stage2BossEncounterTrigger : ITurretTrigger
    {
        public void Initialize(EnemyTurret turret)
        {
            MessageBroker.Default.Receive<Stage2BossEncounter>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => turret.StartShootingAsync())
                .AddTo(turret);
        }
    }
}
