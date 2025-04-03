using System;
using Event.Signal;
using Turret;
using UniRx;

namespace _RAYSER.Scripts.Turret.Trigger
{
    [Serializable]
    public class Stage2StartTrigger : ITurretTrigger
    {
        public void Initialize(EnemyTurret turret)
        {
            MessageBroker.Default.Receive<Stage2Start>()
                .Subscribe(_ => turret.StartShootingAsync())
                .AddTo(turret);
        }
    }
}
