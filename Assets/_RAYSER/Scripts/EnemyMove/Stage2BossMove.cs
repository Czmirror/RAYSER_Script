using DG.Tweening;
using Status;
using UniRx;
using UnityEngine;

namespace EnemyMove
{
    public class Stage2BossMove : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// ボスの初期地点
        /// </summary>
        private Vector3 stage1BossInitialPositionGoal = new Vector3(-830, 0, 0);

        // <summary>
        /// ボスの移動地点上部
        /// </summary>
        private Vector3 stage1BossMoveTop = new Vector3(-830, 50, 0);

        // <summary>
        /// ボスの移動地点下部
        /// </summary>
        private Vector3 stage1BossMoveBottom = new Vector3(-830, -50, 0);

        // <summary>
        /// ボスの初期地点移動スピード
        /// </summary>
        private float moveInitialTime = 3f;

        // <summary>
        /// ボスの移動スピード
        /// </summary>
        private float moveTime = 4f;

        private bool isMoveStart = false;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2Boss
                )
                .Subscribe(_ => Stage2BossInitialMove())
                .AddTo(this);
        }

        private void Stage2BossInitialMove()
        {
            transform.DOMove(stage1BossInitialPositionGoal, moveInitialTime)
                .OnComplete(() => Stage2BossMoveLoop())
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }

        private void Stage2BossMoveLoop()
        {
            transform.DOMove(stage1BossMoveTop, moveTime)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();

            transform.DOMove(stage1BossMoveBottom, moveTime)
                .SetLoops(-1, LoopType.Yoyo)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }
    }
}
