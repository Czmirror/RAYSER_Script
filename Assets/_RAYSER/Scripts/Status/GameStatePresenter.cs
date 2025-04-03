using System;
using Event.Signal;
using UI.Game;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace Status
{
    public class GameStatePresenter : IStartable, IDisposable
    {
        private GameStateService gameStateService;
        readonly CompositeDisposable disposable = new CompositeDisposable();

        public GameStatePresenter(GameStateService gameStateService)
        {
            this.gameStateService = gameStateService;
        }

        void IStartable.Start()
        {
            MessageBroker.Default.Receive<GameStatusReset>()
                .Subscribe(_ => gameStateService.Reset())
                .AddTo(disposable);

            MessageBroker.Default.Receive<Stage2Start>()
                .Subscribe(_ => gameStateService.SetGameStatus(GameState.Stage2))
                .AddTo(disposable);

            MessageBroker.Default.Receive<Stage3Start>()
                .Subscribe(_ => gameStateService.SetGameStatus(GameState.Stage3))
                .AddTo(disposable);


            ContinueGame();
        }

        private void ContinueGame()
        {
            // コンティニュー時に、gameStateServiceからの値を元に、MessageBrokerでステータスを送信する
            var gameState = gameStateService.GetGameStatus();
            Debug.Log("GameStatePresenter ContinueGame: " + gameState);

            switch (gameState)
            {
                case GameState.Stage2:
                    MessageBroker.Default.Publish(new Stage2IntervalStart(TalkEnum.TalkStart));
                    Debug.Log("GameStatePresenter Stage2IntervalStart");
                    break;
                case GameState.Stage3:
                    MessageBroker.Default.Publish(new Stage3IntervalStart(TalkEnum.TalkStart));
                    Debug.Log("GameStatePresenter Stage3IntervalStart");
                    break;
            }
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}
