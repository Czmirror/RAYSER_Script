using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Game
{
    /// <summary>
    /// ゲーム開始時の明転
    /// </summary>
    public class GameStartUIMingTurn : MonoBehaviour
    {
        [SerializeField] private RectTransform gameStartUIRectTransform;
        [SerializeField] private CanvasGroup gameStartUICanvasGroup;
        [SerializeField] private float mingturnTime = 3f;
        private Vector3 _outUIPosition = new Vector3(2000, 0, 0);

        private void Start()
        {
            gameStartUICanvasGroup.alpha = 1;

            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            sequence
                .Append(gameStartUICanvasGroup.DOFade(0f, mingturnTime))
                .AppendCallback(() => { gameStartUIRectTransform.localPosition = _outUIPosition; })
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);

            sequence.Restart();
        }
    }
}
