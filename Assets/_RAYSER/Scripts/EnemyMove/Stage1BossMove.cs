using System;
using Capsule;
using DG.Tweening;
using Status;
using UniRx;
using UnityEngine;

namespace EnemyMove
{
    /// <summary>
    /// ステージ１ボス移動用クラス
    /// </summary>
    public class Stage1BossMove : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// ボスの初期地点
        /// </summary>
        private Vector3 stage1BossInitialPositionGoal = new Vector3(-20, 300, 0);

        // <summary>
        /// ボスの移動地点上部
        /// </summary>
        private Vector3 stage1BossMoveLeft = new Vector3(-20, 300, -55);

        // <summary>
        /// ボスの移動地点下部
        /// </summary>
        private Vector3 stage1BossMoveRight = new Vector3(-20, 300, 55);

        /// <summary>
        /// ボスの初期地点移動スピード
        /// </summary>
        private float moveInitialTime = 3f;

        // <summary>
        /// ボスの移動スピード
        /// </summary>
        private float moveTime = 4f;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage1Boss
                )
                .Subscribe(_ => Stage1BossInitialMove())
                .AddTo(this);
        }

        /// <summary>
        /// ボスの初期移動
        /// </summary>
        private void Stage1BossInitialMove()
        {
            transform.DOMove(stage1BossInitialPositionGoal, moveInitialTime)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();

            transform.DOMove(stage1BossMoveLeft, moveTime)
                .OnComplete(() =>
                {
                    MessageBroker.Default.Publish(new Stage1BossInitialMoveEnd());
                    Stage1BossMoveLoop();
                })
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }

        /// <summary>
        /// ボスの往復移動
        /// </summary>
        private void Stage1BossMoveLoop()
        {
            transform.DOMove(stage1BossMoveRight, moveTime)
                .OnComplete(() => Stage1BossMoveLoop())
                .SetLoops(-1, LoopType.Yoyo)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }
    }
}
