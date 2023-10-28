using System;
using DG.Tweening;
using Event.Signal;
using Status;
using UniRx;
using UnityEngine;

namespace EnemyMove
{
    public class Stage2VerticalDroneMove : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// 登場時間
        /// </summary>
        [SerializeField] private float spawnTime = 30;

        /// <summary>
        /// 初期地点移動スピード
        /// </summary>
        private float moveInitialTime = 3f;

        /// <summary>
        /// 移動場所Y軸
        /// </summary>
        [SerializeField] private float movePositionY = 305;

        private void Start()
        {
            gameObject.SetActive(false);

            // ステージ２になった時点で初期設定を実施
            MessageBroker.Default.Receive<Stage2Start>()
                .Subscribe(_ => Initialize())
                .AddTo(this);
        }

        /// <summary>
        /// ボス初期設定
        /// </summary>
        private void Initialize()
        {
            Observable.Timer(TimeSpan.FromSeconds(spawnTime))
                .Where(_ => _gameStatus.CurrentGameState == GameState.Stage2)
                .Subscribe(_ => { InitialMove(); })
                .AddTo(this);
        }

        private void InitialMove()
        {
            gameObject.SetActive(true);

            transform.DOLocalMoveY(movePositionY, moveInitialTime)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }
    }
}
