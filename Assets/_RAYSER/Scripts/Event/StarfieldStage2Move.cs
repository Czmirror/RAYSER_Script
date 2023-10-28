using DG.Tweening;
using Status;
using UniRx;
using UnityEngine;

namespace Event
{
    public class StarfieldStage2Move : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// 星屑到達地点
        /// </summary>
        [SerializeField] private Vector3 starfieldMoveEndPosition = new Vector3(100, 0, 0);

        /// <summary>
        /// 星屑到達移動時間
        /// </summary>
        [SerializeField] private float starfieldMoveTime = 90f;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2
                )
                .Subscribe(_ => starfieldMoveStart())
                .AddTo(this);

        }

        private void starfieldMoveStart()
        {
            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            sequence
                .Append(transform.DOMove(starfieldMoveEndPosition, starfieldMoveTime))
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);

            sequence.Restart();
        }
    }
}
