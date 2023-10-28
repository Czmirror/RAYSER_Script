using System;
using Event.Signal;
using Target;
using UI.Game;
using UniRx;
using UnityEngine;

namespace Turret
{
    public class Stage3BossTurret : MonoBehaviour
    {
        [SerializeField] private EnemyTarget _enemyTarget;

        /// <summary>
        /// 標的となる自機
        /// </summary>
        [SerializeField] private GameObject targetPlayer;

        /// <summary>
        /// ショットの自動消滅時間
        /// </summary>
        [SerializeField] private float shotInterbalTime = 0.75f;

        /// <summary>
        /// 敵機のビームのゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject enemyBeam;

        private bool IsShotStop = false;

        private void Start()
        {
            _enemyTarget.TargetInitialize(targetPlayer);

            MessageBroker.Default.Receive<Stage3Start>()
                .Subscribe(_ => BossShotStart())
                .AddTo(this);

            MessageBroker.Default.Receive<GameClear>()
                .Subscribe(_ => IsShotStop = true)
                .AddTo(this);
        }

        private void BossShotStart()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(shotInterbalTime))
                .Subscribe(_ => EnemyShot())
                .AddTo(this);
        }

        private void EnemyShot()
        {
            if (IsShotStop)
            {
                return;
            }

            transform.LookAt(_enemyTarget.CurrentTarget().transform.position);
            GameObject _shot = Instantiate(enemyBeam, transform.position, transform.rotation);
        }
    }
}
