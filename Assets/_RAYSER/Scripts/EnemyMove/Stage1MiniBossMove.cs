using System;
using _RAYSER.Scripts.Event.Signal;
using DG.Tweening;
using Event.Signal;
using Status;
using UniRx;
using UnityEngine;

namespace EnemyMove
{
    public class Stage1MiniBossMove : MonoBehaviour
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
        /// 移動場所Y軸
        /// </summary>
        private float movePositionY = 305;

        /// <summary>
        /// 有効フラグ
        /// </summary>
        private bool activate = false;

        /// <summary>
        /// ターゲット
        /// </summary>
        [SerializeField] private GameObject target;

        private void Start()
        {
            if (target == null)
            {
                target = GameObject.Find("RAYSER");
            }

            gameObject.SetActive(false);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage1Boss
                )
                .Subscribe(_ => Stage1MiniBossInitialMove())
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
        private void Stage1MiniBossInitialMove()
        {
            MessageBroker.Default.Publish(new Warning());
            gameObject.SetActive(true);
            activate = true;

            transform.DOLocalMoveY(movePositionY, moveInitialTime)
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }
    }
}
