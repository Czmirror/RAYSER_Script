using _RAYSER.Scripts.Event.Signal;
using DG.Tweening;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class WarningVisible : MonoBehaviour
    {
        private float DurationSeconds = 1f;
        private Ease EaseType;

        private CanvasGroup canvasGroup;

        private void Start()
        {
            this.canvasGroup = this.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            MessageBroker.Default.Receive<Warning>().Subscribe(_ => ViewUI()).AddTo(this);
        }

        private void ViewUI()
        {
            this.canvasGroup.DOFade(1.0f, this.DurationSeconds)
                .SetEase(this.EaseType)
                .SetLoops(3, LoopType.Yoyo)
                .OnComplete(() => { canvasGroup.alpha = 0; })
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }
    }
}
