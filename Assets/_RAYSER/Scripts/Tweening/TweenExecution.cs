using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _RAYSER.Scripts.Tweening
{
    /// <summary>
    /// UIのトゥイーン処理を実行するクラス
    /// </summary>
    public class TweenExecution
    {
        private const float defalutFadoInEndValue = 1f;
        private const float defalutFadoInDuration = 0.3f;
        private const float defalutFadoOutEndValue = 0f;
        private const float defalutFadoOutDuration = 0.3f;

        /// <summary>
        /// フェード実行
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public UniTask Fade(
            CanvasGroup canvasGroup,
            float endValue,
            float duration,
            CancellationToken cancellationToken
        )
        {
            return canvasGroup.DOFade(endValue, duration)
                .Play()
                .ToUniTask(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// フェードイン実行
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public UniTask FadeIn(
            CanvasGroup canvasGroup,
            CancellationToken cancellationToken
        )
        {
            return canvasGroup
                .DOFade(defalutFadoInEndValue, defalutFadoInDuration)
                .Play()
                .ToUniTask(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// フェードアウト実行
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public UniTask FadeOut(
            CanvasGroup canvasGroup,
            CancellationToken cancellationToken
        )
        {
            return canvasGroup.DOFade(defalutFadoOutEndValue, defalutFadoOutDuration)
                .Play()
                .ToUniTask(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// サイズ変更
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public UniTask SizeDelta(
            RectTransform rectTransform,
            Vector2 endValue,
            float duration,
            CancellationToken cancellationToken
        )
        {
            return rectTransform.DOSizeDelta(endValue, duration)
                .Play()
                .ToUniTask(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// X軸への移動
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public UniTask LocalMoveX(
            RectTransform rectTransform,
            float endValue,
            float duration,
            CancellationToken cancellationToken
        )
        {
            return rectTransform.DOLocalMoveX(endValue, duration)
                .Play()
                .ToUniTask(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 文字の非表示
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public UniTask HideText(TextMeshProUGUI textMeshProUGUI, CancellationToken cancellationToken)
        {
            return textMeshProUGUI.DOText(String.Empty, 0)
                .Play()
                .ToUniTask(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 文字の表示
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="text"></param>
        /// <param name="speed"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public UniTask ShowText(TextMeshProUGUI textMeshProUGUI, string text, float speed, CancellationToken cancellationToken)
        {
            return textMeshProUGUI
                .DOText(text, text.Length * speed)
                .SetEase(Ease.Linear)
                .Play()
                .ToUniTask(cancellationToken: cancellationToken);
        }
    }
}
