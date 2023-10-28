using System;
using DG.Tweening;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace Event
{
    public class StarfieldStage3Move : MonoBehaviour
    {
        /// <summary>
        /// 星屑到達地点
        /// </summary>
        private Vector3 starfieldMoveEndPosition = new Vector3(-2000, 0, 0);

        /// <summary>
        /// 星屑到達移動時間
        /// </summary>
        private float starfieldMoveTime = 10f;
        private void Start()
        {
            MessageBroker.Default.Receive<Stage3Start>().Subscribe(_ => starfieldMoveStart()).AddTo(this);
        }

        private void starfieldMoveStart()
        {
            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            sequence
                .Append(transform
                    .DOMove(starfieldMoveEndPosition, starfieldMoveTime)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear))
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .OnComplete(() =>
                {
                    transform.position = Vector3.zero;
                    starfieldMoveStart();
                });

            sequence.Restart();
        }
    }
}
