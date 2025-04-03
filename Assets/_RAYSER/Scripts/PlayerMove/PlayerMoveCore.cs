using System;
using Event.Signal;
using Status;
using UniRx;
using UnityEngine;

namespace PlayerMove
{
    /// <summary>
    /// 自機移動用クラス
    /// </summary>
    public class PlayerMoveCore : MonoBehaviour
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// 自機の基本スピード
        /// </summary>
        [SerializeField] private float basePlayerMoveSpeed = 20;

        /// <summary>
        /// レベルに紐づくスピード
        /// </summary>
        [SerializeField] private float levelSpeed = 5;

        /// <summary>
        /// 設定中のスピードレベルクラス
        /// </summary>
        [SerializeField] private PlayerSpeedLevel _playerSpeedLevel = new PlayerSpeedLevel();
        public PlayerSpeedLevel PlayerSpeedLevel => _playerSpeedLevel;

        /// <summary>
        /// スピードレベルUniRx
        /// </summary>
        [SerializeField] private ReactiveProperty<int> currentSpeedLevel = new ReactiveProperty<int>(1);

        /// <summary>
        /// 外部参照用スピードレベルUniRx
        /// </summary>
        public IObservable<int> currentSpeedLevelObservable => currentSpeedLevel;

        /// <summary>
        /// 設定されている移動クラス
        /// </summary>
        [SerializeField] private IMovable currentMoveType;


        [SerializeField] private FrontViewTiltHandler _tiltHandler;

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Tutorial)
                .Subscribe(_ => TopViewMoveSwitch())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage1)
                .Subscribe(_ => TopViewMoveSwitch())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage2)
                .Subscribe(_ => SideViewMoveSwitch())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage3)
                .Subscribe(_ => FrontViewMoveSwitch())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage2Interval)
                .Subscribe(_ => UserControlStopSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage3Interval)
                .Subscribe(_ => UserControlStopSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.GameClear)
                .Subscribe(_ => UserControlStopSetting())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Gameover)
                .Subscribe(_ => UserControlStopSetting())
                .AddTo(this);

            MessageBroker.Default.Receive<PlayerMoveSpeedLevelUp>().Subscribe(_ => SpeedLevelUp()).AddTo(this);
        }

        private void Update()
        {
            Move();
        }

        private void SpeedLevelUp()
        {
            _playerSpeedLevel.SpeedLevelUp();
            currentSpeedLevel.Value = _playerSpeedLevel.CurrentSpeedLevel();
        }

        public void SetDirection(Vector2 moveValue)
        {
            if (currentMoveType == null)
            {
                return;
            }

            currentMoveType.SetDirection(moveValue);
        }

        public void Move()
        {
            if (currentMoveType == null)
            {
                return;
            }

            currentMoveType.Move();
        }

        /// <summary>
        /// 現在のスピードを返却
        /// </summary>
        /// <returns>ベースのスピードとレベルアップのスピードを加算した値を返却</returns>
        public float CurrentSpeed()
        {
            float speedOfLevelSpeed = CurrentSpeedLevel() * levelSpeed;
            float currentSpeed = basePlayerMoveSpeed + speedOfLevelSpeed;
            return currentSpeed;
        }

        /// <summary>
        /// 現在のスピードレベルを返却
        /// </summary>
        /// <returns>現在のスピードレベル</returns>
        public int CurrentSpeedLevel()
        {
            return _playerSpeedLevel.CurrentSpeedLevel();
        }

        /// <summary>
        /// ユーザーコントロール停止設定
        /// </summary>
        private void UserControlStopSetting()
        {
            currentMoveType = null;
        }

        /// <summary>
        /// トップビュー用移動クラス切り替え設定
        /// </summary>
        private void TopViewMoveSwitch()
        {
            currentMoveType = new TopViewMove();
            currentMoveType.Initialize(this);
        }

        /// <summary>
        /// サイドビュー用移動クラス切り替え設定
        /// </summary>
        private void SideViewMoveSwitch()
        {
            currentMoveType = new SideViewMove();
            currentMoveType.Initialize(this);
        }

        /// <summary>
        /// フロントビュー用移動クラス切り替え設定
        /// </summary>
        private void FrontViewMoveSwitch()
        {
            currentMoveType = new FrontViewMove();
            currentMoveType.Initialize(this);
            ((FrontViewMove) currentMoveType).SetFrontViewTiltHandler(_tiltHandler);
        }
    }
}
