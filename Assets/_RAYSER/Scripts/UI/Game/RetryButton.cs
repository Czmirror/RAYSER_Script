using DG.Tweening;
using Status;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Game
{
    public class RetryButton : MonoBehaviour
    {
        [SerializeField] private RectTransform gameUIMingTurnRectTransform;
        [SerializeField] private CanvasGroup gameUIMingTurnCanvasGroup;
        [SerializeField] private float mingturnTime = 3f;
        [SerializeField] private RetryButtonType retryButtonType;

        private Vector3 _inUIPosition = Vector3.zero;

        // enum retry,continue
        public enum RetryButtonType
        {
            Retry,
            Continue
        }

        public void PushButton()
        {
            gameUIMingTurnRectTransform.localPosition = _inUIPosition;
            // DOTweenシーケンスセット
            var sequence = DOTween.Sequence();

            sequence
                .Append(gameUIMingTurnCanvasGroup.DOFade(1f, mingturnTime))
                .OnComplete(() => GameRetry())
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject);

            sequence.Restart();
        }

        private void GameRetry()
        {
            if (retryButtonType == RetryButtonType.Retry)
            {
                MessageBroker.Default.Publish(new GameStatusReset());
            }

            DOTween.Clear(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
