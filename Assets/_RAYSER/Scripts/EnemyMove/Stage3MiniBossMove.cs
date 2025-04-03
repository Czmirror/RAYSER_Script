using System;
using _RAYSER.Scripts.Event.Signal;
using DG.Tweening;
using Event.Signal;
using Status;
using UniRx;
using UnityEngine;

namespace EnemyMove
{
    public class Stage3MiniBossMove: MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// ミニボス登場時間
        /// </summary>
        private int spawnTime = 30;

        /// <summary>
        /// 初期地点移動スピード
        /// </summary>
        private float moveInitialTime = 3f;

        /// <summary>
        /// 移動場所X軸
        /// </summary>
        private float movePositionX = -1100;

        /// <summary>
        /// 有効フラグ
        /// </summary>
        private bool activate = false;

        /// <summary>
        /// ターゲット
        /// </summary>
        [SerializeField] private GameObject target;

        /// <summary>
        /// 移動するポイントのゲームオブジェクトの配列
        /// </summary>
        [SerializeField] private GameObject[] movePointArray;

        /// <summary>
        /// DOTweenのシーケンス
        /// </summary>
        private Sequence _sequence;

        /// <summary>
        /// 移動速度
        /// </summary>
        private float _moveSpeed = 1f;

        private void Start()
        {
            if (target == null)
            {
                target = GameObject.Find("RAYSER");
            }

            gameObject.SetActive(false);

            // ステージ3になった時点でボス初期設定を実施
            MessageBroker.Default.Receive<Stage3Start>()
                .Subscribe(_ => Initialize())
                .AddTo(this);
        }

        /// <summary>
        /// ボス初期設定
        /// </summary>
        private void Initialize()
        {
            Observable.Timer(TimeSpan.FromSeconds(spawnTime))
                .Where(_ => _gameStatus.CurrentGameState == GameState.Stage3)
                .Subscribe(_ => { MiniBossInitialMove(); })
                .AddTo(this);
        }

        private void Update()
        {
            if (activate == false)
            {
                return;
            }

            transform.LookAt(target.transform);
        }

        /// <summary>
        /// ボスの初期移動
        /// </summary>
        private void MiniBossInitialMove()
        {
            MessageBroker.Default.Publish(new Warning());
            gameObject.SetActive(true);
            activate = true;

            transform.DOLocalMoveX(movePositionX, moveInitialTime)
                .OnComplete(() => { Move(); })
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }

        private void Move()
        {
            // DOTweenシーケンスセット
            _sequence = DOTween.Sequence();

            foreach (var movePoint in movePointArray)
            {
                _sequence.Append(transform.DOPath(
                    new[] { movePoint.transform.position },
                    _moveSpeed,
                    PathType.Linear,
                    gizmoColor: Color.red
                ));
            }

            _sequence
                .AppendInterval(1f)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .OnComplete(() => { Move(); });

            _sequence.Restart();
        }
    }
}
