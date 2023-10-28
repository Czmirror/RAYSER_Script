using System;
using Event;
using Status;
using UnityEngine;


namespace BGM
{
    using UniRx;
    using UniRx.Triggers;

    /// <summary>
    /// 旧BGM切り替えクラス
    /// </summary>
    public class BGMSwitch : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// AudioSourceコンポーネント
        /// </summary>
        [SerializeField] private AudioSource _audioSource;

        /// <summary>
        /// ステージ１BGM
        /// </summary>
        [SerializeField] private AudioClip stage1BGM;

        /// <summary>
        /// ステージ１ボスBGM
        /// </summary>
        [SerializeField] private AudioClip stage1BossBGM;

        /// <summary>
        /// ステージ２BGM
        /// </summary>
        [SerializeField] private AudioClip stage2BGM;

        /// <summary>
        /// ステージ２ボスBGM
        /// </summary>
        [SerializeField] private AudioClip stage2BossBGM;

        /// <summary>
        /// ステージ３BGM
        /// </summary>
        [SerializeField] private AudioClip stage3BGM;

        /// <summary>
        /// エンディングBGM
        /// </summary>
        [SerializeField] private AudioClip endingBGM;

        /// <summary>
        /// ゲームオーバーBGM
        /// </summary>
        [SerializeField] private AudioClip gameoverBGM;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage1
                )
                .Subscribe(_ => Stage1BGMSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage1Boss
                )
                .Subscribe(_ => Stage1BossBGMSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2Interval
                )
                .Subscribe(_ =>
                {
                    _audioSource.Stop();
                })
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2
                )
                .Subscribe(_ => Stage2BGMSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage2Boss
                )
                .Subscribe(_ => Stage2BossBGMSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage3Interval
                )
                .Subscribe(_ =>
                {
                    _audioSource.Stop();
                })
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Stage3
                )
                .Subscribe(_ => Stage3BGMSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.GameClear
                )
                .Subscribe(_ => EndingBGMSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x =>
                    x == GameState.Gameover
                )
                .Subscribe(_ => GameOverBGMSetting())
                .AddTo(this);
        }

        private void SettingBGM(AudioClip _bgm)
        {
            _audioSource.Stop();
            _audioSource.clip = _bgm;
            _audioSource.Play();
        }

        private void Stage1BGMSetting()
        {
            SettingBGM(stage1BGM);
        }

        private void Stage1BossBGMSetting()
        {
            SettingBGM(stage1BossBGM);
        }

        private void Stage2BGMSetting()
        {
            SettingBGM(stage2BGM);
        }

        private void Stage2BossBGMSetting()
        {
            SettingBGM(stage2BossBGM);
        }

        private void Stage3BGMSetting()
        {
            SettingBGM(stage3BGM);
        }

        private void EndingBGMSetting()
        {
            SettingBGM(endingBGM);
        }

        private void GameOverBGMSetting()
        {
            _audioSource.loop = false;
            SettingBGM(gameoverBGM);
        }
    }
}
