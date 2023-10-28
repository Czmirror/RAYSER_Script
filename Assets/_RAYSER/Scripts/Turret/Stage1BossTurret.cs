using System;
using Event.Signal;
using UI.Game;
using UniRx;
using UnityEngine;

namespace Turret
{
    public class Stage1BossTurret : MonoBehaviour
    {
        /// <summary>
        /// ショットの自動消滅時間
        /// </summary>
        [SerializeField] private float shotInterbalTime = 1.5f;

        /// <summary>
        /// 敵機のビームのゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject enemyBeam;
        private void Start()
        {
            MessageBroker.Default.Receive<Stage1BossEncounter>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => BossShotStart())
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
            GameObject _shot = Instantiate(enemyBeam, transform.position, transform.rotation);
        }
    }
}
