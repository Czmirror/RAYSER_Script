using _RAYSER.Scripts.Event.Signal;
using Event;
using Event.Signal;
using UI.Game;
using UniRx;
using UnityEngine;
using VContainer;

namespace Status
{
    /// <summary>
    /// ゲームステータス（独自UniRx）
    /// </summary>
    [SerializeField]
    public class GameStateReactiveProperty : ReactiveProperty<GameState>
    {
        public GameStateReactiveProperty()
        {
        }

        public GameStateReactiveProperty(GameState init) : base(init)
        {
        }
    }

    /// <summary>
    /// ゲームステータス
    /// </summary>
    public class GameStatus : MonoBehaviour
    {
        [Inject] private GameStateService gameStateService;

        /// <summary>
        /// エディタ把握用パラメーター
        /// </summary>
        [SerializeField] private GameState currentGameState;

        public GameState CurrentGameState
        {
            get { return CurrentGameStateReactiveProperty.Value; }
        }

        public readonly GameStateReactiveProperty CurrentGameStateReactiveProperty = new GameStateReactiveProperty();

        private void Start()
        {
            if (gameStateService != null && gameStateService.GetGameStatus() == GameState.Stage2)
            {
                SetGameStatus(GameState.Stage2Interval);
            }
            else if (gameStateService != null && gameStateService.GetGameStatus() == GameState.Stage3)
            {
                SetGameStatus(GameState.Stage3Interval);
            }
            else
            {
                SetGameStatus(GameState.Gamestart);
            }
            // SetGameStatus(GameState.Gamestart);

            MessageBroker.Default.Receive<TutorialMove>().Subscribe(_ => SetGameStatus(GameState.Tutorial))
                .AddTo(this);

            MessageBroker.Default.Receive<GameStartEventEnd>().Subscribe(_ => SetGameStatus(GameState.Stage1))
                .AddTo(this);

            MessageBroker.Default.Receive<Stage1BossEncounter>().Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => SetGameStatus(GameState.Stage1Boss)).AddTo(this);

            MessageBroker.Default.Receive<Stage2IntervalStart>().Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => SetGameStatus(GameState.Stage2Interval)).AddTo(this);

            MessageBroker.Default.Receive<Stage2Start>().Subscribe(_ => SetGameStatus(GameState.Stage2)).AddTo(this);

            MessageBroker.Default.Receive<Stage2BossEncounter>().Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => SetGameStatus(GameState.Stage2Boss)).AddTo(this);

            MessageBroker.Default.Receive<Stage3IntervalStart>().Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => SetGameStatus(GameState.Stage3Interval)).AddTo(this);

            MessageBroker.Default.Receive<Stage3Start>().Subscribe(_ => SetGameStatus(GameState.Stage3)).AddTo(this);

            MessageBroker.Default.Receive<GameClear>().Where(x => x._talk == TalkEnum.TalkStart)
                .Subscribe(_ => SetGameStatus(GameState.GameClear)).AddTo(this);

            MessageBroker.Default.Receive<Gameover>().Subscribe(_ => SetGameStatus(GameState.Gameover)).AddTo(this);
        }

        private void SetGameStatus(GameState gameState)
        {
            currentGameState = gameState;
            CurrentGameStateReactiveProperty.Value = gameState;
        }
    }
}
