using System;
using Capsule;
using Status;
using Target;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemySpawn
{
    /// <summary>
    /// ステージ１ボス戦闘中に出てくる敵機発生用クラス
    /// </summary>
    public class Stage1BossEnemySpawner : MonoBehaviour
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
        private int enemySpawnTime = 2;

        /// <summary>
        /// 敵機生成可能状態
        /// </summary>
        private bool enemySpawnReady = false;

        private void Start()
        {
            MessageBroker.Default.Receive<Stage1BossInitialMoveEnd>()
                .Subscribe(_ => enemySpawnReady = true)
                .AddTo(this);

            Observable
                .Interval(TimeSpan.FromSeconds(enemySpawnTime))
                .Where(_ => enemySpawnReady)
                .Where(_ => _gameStatus.CurrentGameState == GameState.Stage1Boss)
                .Subscribe(_ =>
                {
                    EnemySpawn();
                }).AddTo(this);
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
