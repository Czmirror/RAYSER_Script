using System.Globalization;
using _RAYSER.Scripts.Score;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UniRx;

namespace Score
{
    /// <summary>
    /// スコア表示処理
    /// </summary>
    public class PresenterScore : MonoBehaviour
    {
        [SerializeField] private ScoreCounter _scoreCounter;

        /// <summary>
        /// 更新対象UI
        /// </summary>
        [SerializeField] private TextMeshProUGUI scoreUI;

        /// <summary>
        /// UIアニメーション時間
        /// </summary>
        [SerializeField] private float tweenTime = 0.1f;

        private void Start()
        {
            _scoreCounter.ScoreObservable.Subscribe(x => RefreshUI(x)).AddTo(this);
        }

        private void RefreshUI(float score)
        {
            scoreUI.text = score.ToString();
        }
    }
}
