using System;
using Status;
using Target;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemySpawn
{
    using UniRx;

    /// <summary>
    /// ステージ１隕石生成処理
    /// </summary>
    public class Stage1AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// 標的となる自機
        /// </summary>
        [SerializeField] private GameObject targetPlayer;

        /// <summary>
        /// 生成対象の敵機
        /// </summary>
        [SerializeField] private GameObject[] spawnEnemy;

        /// <summary>
        /// 敵機生成間隔時間
        /// </summary>
        private int enemySpawnMinTime = 12;
        private int enemySpawnMaxTime = 24;

        private void Start()
        {
            Observable
                .Interval(TimeSpan.FromSeconds(Random.Range(enemySpawnMinTime, enemySpawnMaxTime)))
                .Where(_ => _gameStatus.CurrentGameState == GameState.Stage1 || _gameStatus.CurrentGameState == GameState.Stage1Boss)
                .Subscribe(_ =>
                {
                    EnemySpawn();
                })
                .AddTo(this);
        }

        private void EnemySpawn()
        {
            // ランダムに敵機を選択
            var enemyObject = spawnEnemy[Random.Range(0, spawnEnemy.Length)];

            // 敵機を生成
            var enemy = Instantiate(enemyObject, transform.position, Quaternion.identity);

            // 生成された敵機にターゲットを設定
            if (enemy.gameObject.TryGetComponent(out IEnemyTarget enemyTarget))
            {
                enemyTarget.TargetInitialize(targetPlayer);
            }
        }
    }
}
