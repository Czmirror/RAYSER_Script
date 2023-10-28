using System;
using UnityEngine;

namespace Turret
{
    using UniRx;

    /// <summary>
    /// 敵機の射撃処理
    /// </summary>
    public class EnemyBeamTurret : MonoBehaviour
    {
        /// <summary>
        /// ショットインターバル
        /// </summary>
        [SerializeField] private float shotInterbalTime = 1.5f;

        /// <summary>
        /// 敵機のビームのゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject enemyBeam;

        private void Start()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(shotInterbalTime))
                .Subscribe(_ => { EnemyShot(); })
                .AddTo(this);
        }


        /// <summary>
        /// 射撃処理
        /// </summary>
        private void EnemyShot()
        {
            // 親オブジェクトが非表示の場合は射撃処理停止
            if (transform.root.gameObject.activeSelf == false)
            {
                return;
            }

            GameObject _shot = Instantiate(enemyBeam, transform.position, transform.rotation);
        }
    }
}
