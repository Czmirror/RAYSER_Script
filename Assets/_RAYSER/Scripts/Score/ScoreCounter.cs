using System;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace _RAYSER.Scripts.Score
{
    /// <summary>
    /// スコア加算処理用クラス
    /// </summary>
    public class ScoreCounter : MonoBehaviour
    {
        /// <summary>
        /// スコアUniRx
        /// </summary>
        [SerializeField] private ReactiveProperty<int> scoreReactiveProperty = new ReactiveProperty<int>(0);

        /// <summary>
        /// 外部参照用スコアUniRx
        /// </summary>
        public IObservable<int> ScoreObservable => scoreReactiveProperty;

        private void Start()
        {
            MessageBroker.Default.Receive<ScoreAccumulation>().Subscribe(x => ScoreCount(x.Score)).AddTo(this);
        }

        /// <summary>
        /// スコア加算処理
        /// </summary>
        /// <param name="score">スコア</param>
        private void ScoreCount(int score)
        {
            scoreReactiveProperty.Value += score;
        }
    }
}
