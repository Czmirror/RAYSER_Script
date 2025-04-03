using System;
using Event.Signal;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace _RAYSER.Scripts.Score
{
    public class ScoreDataPresenter : IStartable, IDisposable
    {
        readonly ScoreService scoreService;
        readonly CompositeDisposable disposable = new CompositeDisposable();

        public ScoreDataPresenter(ScoreService scoreService)
        {
            this.scoreService = scoreService;
        }

        void IStartable.Start()
        {
            scoreService.ShowScore();

            MessageBroker.Default.Receive<ScoreAccumulation>()
                .Subscribe(x => this.ScoreUICalculateRefreshUI(x.Score))
                .AddTo(disposable);
        }

        private void ScoreUICalculateRefreshUI(int score)
        {
            scoreService.AddScore(score);
            scoreService.ShowScore();
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}
