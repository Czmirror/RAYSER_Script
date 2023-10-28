using System;
using Event.Signal;
using Status;
using UI.Game;
using UniRx;
using UnityEngine;

namespace EnemySpawn
{
    /// <summary>
    /// ステージ１ボス登場処理
    /// </summary>
    public class Stage1BossSpawner : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// ステージ１ボス登場時間
        /// </summary>
        private int stage1BossSpawnTime = 50;

        private void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(stage1BossSpawnTime))
                .Where(_ => _gameStatus.CurrentGameState == GameState.Stage1)
                .Subscribe(_ => { Stage1BossSpawn(); })
                .AddTo(this);
        }

        /// <summary>
        /// ステージ１ボス登場
        /// </summary>
        private void Stage1BossSpawn()
        {
            MessageBroker.Default.Publish(new Warning());
            MessageBroker.Default.Publish(new Stage1BossEncounter(TalkEnum.TalkStart));
        }
    }
}
