using System;
using Event.Signal;
using Status;
using UI.Game;
using UniRx;
using UnityEngine;

namespace Explosion
{
    /// <summary>
    /// ステージ３ボス撃破時の爆破演出用クラス
    /// </summary>
    public class Stage3BossExplosion:MonoBehaviour
    {
        [SerializeField] private GameObject explosion;
        private void Start()
        {
            MessageBroker.Default.Receive<GameClear>().Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => Explosion()).AddTo(this);
        }

        private void Explosion()
        {
            var explosionInstantiate = Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }
}
