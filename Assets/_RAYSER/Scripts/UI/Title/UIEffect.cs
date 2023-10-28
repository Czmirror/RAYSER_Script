using System.Threading;
using _RAYSER.Scripts.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

namespace _RAYSER.Scripts.UI
{
    /// <summary>
    /// UIエフェクト管理クラス
    /// </summary>
    public class UIEffect : IUIEffect
    {
        TweenExecution _tweenExecution = new TweenExecution();
        private IUIEffect _iuiImplementation;

        public TweenExecution TweenExecution
        {
            get => _iuiImplementation.TweenExecution;
            set => _iuiImplementation.TweenExecution = value;
        }

        public void SetAlphaZero(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0;
        }

        public void SetSizeDeltaZero(RectTransform rectTransform)
        {
            rectTransform.sizeDelta = Vector2.zero;
        }

        public UniTask FadeIn(CanvasGroup canvasGroup, CancellationToken cancellationToken)
        {
            return _tweenExecution.FadeIn(canvasGroup, cancellationToken);
        }


        public UniTask FadeOut(CanvasGroup canvasGroup, CancellationToken cancellationToken)
        {
            return _tweenExecution.FadeOut(canvasGroup, cancellationToken);
        }

        public UniTask Fade(
            CanvasGroup canvasGroup,
            float endValue,
            float duration,
            CancellationToken cancellationToken
        )
        {
            return _tweenExecution.Fade(canvasGroup, endValue, duration, cancellationToken);
        }

        public UniTask SizeDelta(RectTransform rectTransform, Vector2 endValue, float duration,
            CancellationToken cancellationToken)
        {
            return _tweenExecution.SizeDelta(rectTransform, endValue, duration, cancellationToken);
        }

        public UniTask LocalMoveX(RectTransform rectTransform, float endValue, float duration,
            CancellationToken cancellationToken)
        {
            return _tweenExecution.LocalMoveX(rectTransform, endValue, duration, cancellationToken);
        }
    }
}
