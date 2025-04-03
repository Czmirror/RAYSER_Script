using Cysharp.Threading.Tasks;
using Status;
using Target;
using UniRx;
using UnityEngine;

namespace Turret
{
    public class PlayerLaserTurret : MonoBehaviour, ITurret
    {
        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// 自機のターゲティング
        /// </summary>
        [SerializeField] private PlayerTargeting _playerTargeting;

        /// <summary>
        /// トップビュー用ターゲティング
        /// </summary>
        [SerializeField] private PlayerTargeting _playerTargetingTop;

        /// <summary>
        /// サイドビュー用ターゲティング
        /// </summary>
        [SerializeField] private PlayerTargeting _playerTargetingSide;

        /// <summary>
        /// フロントビュー用ターゲティング
        /// </summary>
        [SerializeField] private PlayerTargeting _playerTargetingFront;

        /// <summary>
        /// 現在設定されているレーザーレベルクラス
        /// </summary>
        [SerializeField] private PlayerLaserLevel _playerLaserLevel;

        /// <summary>
        /// レーザーのインターバルタイム
        /// </summary>
        [SerializeField] private float laserInterval = 0.5f;

        /// <summary>
        /// レーザーのインターバル中の時間
        /// </summary>
        [SerializeField] private float laserIntervalCurrent = 0.0f;

        /// <summary>
        /// 射撃状態
        /// </summary>
        private bool isFire = false;

        /// <summary>
        /// イベント状態
        /// </summary>
        private bool isEvent = true;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Gamestart)
                .Subscribe(_ => isEvent = true)
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage1)
                .Subscribe(_ =>
                {
                    isEvent = false;
                    _playerTargeting = _playerTargetingTop;
                })
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage2)
                .Subscribe(_ =>
                {
                    isEvent = false;
                    _playerTargeting = _playerTargetingSide;
                })
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage3)
                .Subscribe(_ =>
                {
                    isEvent = false;
                    _playerTargeting = _playerTargetingFront;
                })
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage2Interval)
                .Subscribe(_ => isEvent = true)
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage3Interval)
                .Subscribe(_ => isEvent = true)
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.GameClear)
                .Subscribe(_ => isEvent = true)
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Gameover)
                .Subscribe(_ => isEvent = true)
                .AddTo(this);
        }

        public async UniTask StartShootingAsync()
        {
            isFire = true;
            await UniTask.CompletedTask;
        }

        public void StopShooting()
        {
            isFire = false;
        }

        /// <summary>
        /// レーザーを有効にする
        /// </summary>
        public void LaserEnable()
        {
            isEvent = false;
            gameObject.SetActive(true);
        }

        private void LaserShot()
        {
            if (laserIntervalCurrent > 0)
            {
                return;
            }

            Laser _laser = Instantiate(this._playerLaserLevel.CurrentLaser());
            _laser.Initialize(transform, _playerTargeting);
            laserIntervalCurrent = laserInterval;
        }

        private void Update()
        {
            if (isEvent == true)
            {
                return;
            }

            if (isFire == false)
            {
                return;
            }

            if (laserIntervalCurrent > 0)
            {
                LaserCoolDown();
                return;
            }

            LaserShot();
        }

        /// <summary>
        /// レーザー発射のインターバル
        /// </summary>
        private void LaserCoolDown()
        {
            laserIntervalCurrent -= Time.deltaTime;
        }

        public PlayerTargeting CurrentPlayerTargeting()
        {
            return _playerTargeting;
        }

        public async UniTask CleanupAsync()
        {
            Destroy(gameObject);
            await UniTask.CompletedTask;
        }
    }
}
