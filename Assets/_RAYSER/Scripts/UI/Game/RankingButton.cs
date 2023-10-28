using _RAYSER.Scripts.Score;
using Score;
using UniRx;
using UnityEngine;

namespace UI.Game
{
    public class RankingButton : MonoBehaviour
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private int score;

        private void Start()
        {
            _scoreCounter.ScoreObservable.Subscribe(x => score = x).AddTo(this);
        }

        public void CallRanking()
        {
            naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
        }
    }
}
