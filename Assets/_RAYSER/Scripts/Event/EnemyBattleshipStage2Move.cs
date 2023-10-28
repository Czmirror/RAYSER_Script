using System;
using DG.Tweening;
using Event.Signal;
using Status;
using UI.Game;
using UniRx;
using UnityEngine;

namespace Event
{
    /// <summary>
    /// ステージ２戦艦移動用クラス
    /// </summary>
    public class EnemyBattleshipStage2Move : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// 敵戦艦到達地点
        /// </summary>
        [SerializeField] private Vector3 enemyBattleshipMoveEndPosition = new Vector3(-700, 0, -100);

        /// <summary>
        /// 敵戦艦到達移動時間
        /// </summary>
        [SerializeField] private float enemyBattleshipMoveTime = 90f;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2
                )
                .Subscribe(_ => enemyBattleshipMoveStart())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage3Interval
                )
                .Subscribe(_ => enemyBattleshipMoveStage3())
                .AddTo(this);
        }

        private void enemyBattleshipMoveStart()
        {
            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            sequence
                .Append(transform.DOMove(enemyBattleshipMoveEndPosition, enemyBattleshipMoveTime))
                .OnComplete(() => { enemyBattleshipMoveEnd(); })
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);

            sequence.Restart();
        }

        private void enemyBattleshipMoveEnd()
        {
            if (_gameStatus.CurrentGameState != GameState.Stage2)
            {
                return;
            }

            MessageBroker.Default.Publish(new Stage2BossEncounter(TalkEnum.TalkStart));
        }

        private void enemyBattleshipMoveStage3()
        {
            transform.position = enemyBattleshipMoveEndPosition;
        }
    }
}
