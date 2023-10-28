using System;
using Target;
using UniRx;
using UnityEngine;

namespace Turret
{
    /// <summary>
    /// ステージ2砲台弾発射クラス
    /// </summary>
    public class Stage2MainGunTurret: MonoBehaviour
    {
        /// <summary>
        /// ショットインターバル
        /// </summary>
        [SerializeField] private float shotInterbalTime = 1.0f;

        /// <summary>
        /// 敵機のビームのゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject enemyBeam;

        [SerializeField] private GameObject target = null;

        [SerializeField] private EnemyLockOn _enemyLockOn;

        private void Start()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(shotInterbalTime))
                .Subscribe(_ => { EnemyShot(); }).AddTo(this);
        }

        private void EnemyShot()
        {
            if (_enemyLockOn.CurrentTarget() == null)
            {
                return;
            }
            GameObject _shot = Instantiate(enemyBeam, transform.position, transform.rotation);
        }
    }
}
