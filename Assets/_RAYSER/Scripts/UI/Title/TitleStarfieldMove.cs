using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Title
{
    public class TitleStarfieldMove : MonoBehaviour
    {

        /// <summary>
        /// 星屑到達地点
        /// </summary>
        private Vector3 starfieldMoveEndPosition = new Vector3(-2000, 0, 0);

        /// <summary>
        /// 星屑到達移動時間
        /// </summary>
        private float starfieldMoveTime = 180f;

        private Vector3 initialPosition;
        private void Start()
        {
            initialPosition = transform.position;
            starfieldMoveStart();
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
                    transform.position = initialPosition;
                    starfieldMoveStart();
                });

            sequence.Restart();
        }
    }
}
