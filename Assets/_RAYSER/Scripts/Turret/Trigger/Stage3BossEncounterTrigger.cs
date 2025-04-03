using System;
using Event.Signal;
using Turret;
using UniRx;

namespace _RAYSER.Scripts.Turret.Trigger
{
    /// <summary>
    /// ステージ3ボスエンカウントトリガー
    /// </summary>
    [Serializable]
    public class Stage3BossEncounterTrigger : ITurretTrigger
    {
        public void Initialize(EnemyTurret turret)
        {
            MessageBroker.Default.Receive<Stage3Start>()
                .Subscribe(_ => turret.StartShootingAsync())
                .AddTo(turret);

            MessageBroker.Default.Receive<GameClear>()
                .Subscribe(_ => { turret.StopShooting(); })
                .AddTo(turret);
        }
    }
}
