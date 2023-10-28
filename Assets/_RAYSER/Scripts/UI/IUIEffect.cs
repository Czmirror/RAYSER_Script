using System.Threading;
using _RAYSER.Scripts.Tweening;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace _RAYSER.Scripts.UI
{
    public interface IUIEffect
    {
        TweenExecution TweenExecution { get; set; }

        /// <summary>
        /// UIのalphaを0にする
        /// </summary>
        void SetAlphaZero(CanvasGroup canvasGroup);

        /// <summary>
        /// 外部から受け取ったRectTransformのsizeDeltaを0にする
        /// </summary>
        void SetSizeDeltaZero(RectTransform rectTransform);

        /// <summary>
        /// UIのフェードイン
        /// </summary>
        UniTask FadeIn(CanvasGroup canvasGroup, CancellationToken cancellationToken);

        /// <summary>
        /// UIのフェードアウト
        /// </summary>
        UniTask FadeOut(CanvasGroup canvasGroup, CancellationToken cancellationToken);

        /// <summary>
        /// UIのフェード処理
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask Fade(CanvasGroup canvasGroup,float endValue, float duration, CancellationToken cancellationToken);

        /// <summary>
        /// UIのサイズ変更
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask SizeDelta(RectTransform rectTransform, Vector2 endValue, float duration, CancellationToken cancellationToken);
    }
}
