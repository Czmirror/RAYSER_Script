using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Game
{
    public class RetryButton : MonoBehaviour
    {
        [SerializeField] private RectTransform gameUIMingTurnRectTransform;
        [SerializeField] private CanvasGroup gameUIMingTurnCanvasGroup;
        [SerializeField] private float mingturnTime = 3f;

        private Vector3 _inUIPosition = Vector3.zero;

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
            DOTween.Clear(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
