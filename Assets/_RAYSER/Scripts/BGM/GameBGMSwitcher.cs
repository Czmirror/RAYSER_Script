using Status;
using UniRx;
using UnityEngine;

namespace BGM
{
    /// <summary>
    /// ゲームシーンBGM制御クラス
    /// </summary>
    public class GameBGMSwitcher : MonoBehaviour, ILoopSoundPlayer
    {
        [SerializeField] private LoopSoundPlayer _loopSoundPlayer;

        public LoopSoundPlayer LoopSoundPlayer
        {
            get => _loopSoundPlayer;
            set => _loopSoundPlayer = value;
        }

        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// ゲームスタート
        /// </summary>
        [SerializeField] private LoopSound gameStartBGM;

        /// <summary>
        /// ステージ１BGM
        /// </summary>
        [SerializeField] private LoopSound stage1BGM;

        /// <summary>
        /// ステージ１ボスBGM
        /// </summary>
        [SerializeField] private LoopSound stage1BossBGM;

        /// <summary>
        /// ステージ２BGM
        /// </summary>
        [SerializeField] private LoopSound stage2BGM;

        /// <summary>
        /// ステージ２ボスBGM
        /// </summary>
        [SerializeField] private LoopSound stage2BossBGM;

        /// <summary>
        /// ステージ３BGM
        /// </summary>
        [SerializeField] private LoopSound stage3BGM;

        /// <summary>
        /// エンディングBGM
        /// </summary>
        [SerializeField] private LoopSound endingBGM;

        /// <summary>
        /// ゲームオーバーBGM
        /// </summary>
        [SerializeField] private LoopSound gameoverBGM;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Gamestart
                )
                .Subscribe(_ => LoopSoundPlayer.ChangeLoopSound(gameStartBGM))
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage1
                )
                .Subscribe(_ => LoopSoundPlayer.ChangeLoopSound(stage1BGM))
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage1Boss
                )
                .Subscribe(_ => LoopSoundPlayer.ChangeLoopSound(stage1BossBGM))
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2Interval
                )
                .Subscribe(_ =>
                {
                    LoopSoundPlayer.Stop();
                })
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2
                )
                .Subscribe(_ => LoopSoundPlayer.ChangeLoopSound(stage2BGM))
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2Boss
                )
                .Subscribe(_ => LoopSoundPlayer.ChangeLoopSound(stage2BossBGM))
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage3Interval
                )
                .Subscribe(_ =>
                {
                    LoopSoundPlayer.Stop();
                })
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage3
                )
                .Subscribe(_ => LoopSoundPlayer.ChangeLoopSound(stage3BGM))
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.GameClear
                )
                .Subscribe(_ => LoopSoundPlayer.ChangeLoopSound(endingBGM))
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Gameover
                )
                .Subscribe(_ => LoopSoundPlayer.ChangeLoopSound(gameoverBGM))
                .AddTo(this);
        }
    }
}
