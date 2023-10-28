using System;
using UniRx;
using UnityEngine;

namespace EnemySpawn
{
    public class EnemyDisappear : MonoBehaviour
    {
        /// <summary>
        /// 敵機活動時間
        /// </summary>
        private int enemyActiveTime = 30;

        private void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(enemyActiveTime)).Subscribe(_ => { EnemyStop(); })
                .AddTo(this);
        }
        private void EnemyStop()
        {
            gameObject.SetActive(false);
        }
    }
}
