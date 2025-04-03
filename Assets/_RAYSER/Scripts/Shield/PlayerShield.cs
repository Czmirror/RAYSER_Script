using System;
using System.Threading;
using _RAYSER.Scripts.Bomb;
using _RAYSER.Scripts.Event.Signal;
using Cysharp.Threading.Tasks;
using Damage;
using Event;
using Event.Signal;
using MessagePipe;
using Status;
using UniRx;
using UnityEngine;

namespace Shield
{
    /// <summary>
    /// 自機のシールド
    /// </summary>
    public class PlayerShield : MonoBehaviour, IShield
    {
        [SerializeField] private GameObject player;

        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// 爆発処理のゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject explosionDestruction;

        /// <summary>
        /// ダメージ処理のゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject explosionDamage;

        /// <summary>
        /// シールドUniRx
        /// </summary>
        [SerializeField] private ReactiveProperty<float> shield = new ReactiveProperty<float>(100f);

        public float CurrentShield => shield.Value;

        /// <summary>
        /// 外部参照用シールドUniRx
        /// </summary>
        public IObservable<float> ShieldObservable => shield;

        /// <summary>
        /// シールド最大値UniRx
        /// </summary>
        [SerializeField] private ReactiveProperty<float> maxShield = new ReactiveProperty<float>(100);

        /// <summary>
        /// シールド危険値現象判定値
        /// </summary>
        private float shieldDangerValue = 50;

        /// <summary>
        /// 外部参照用シールド最大値UniRx
        /// </summary>
        public IObservable<float> maxShieldObservable => maxShield;

        /// <summary>
        /// イベント時無敵状態
        /// </summary>
        private bool isEventInvincible = false;

        /// <summary>
        /// ダメージ後無敵状態UniRx
        /// </summary>
        [SerializeField] private ReactiveProperty<bool> isDamageInvincible = new ReactiveProperty<bool>(false);

        /// <summary>
        /// 外部参照用ダメージ後無敵状態UniRx
        /// </summary>
        public IObservable<bool> IsDamageInvincibleObservable => isDamageInvincible;

        /// <summary>
        /// ダメージ後無敵状態有効時間
        /// </summary>
        private float isDamageInvincibleTime = 3f;

        /// <summary>
        /// ローリング無敵状態UniRx
        /// </summary>
        [SerializeField] private ReactiveProperty<bool> isRollingInvincible = new ReactiveProperty<bool>(false);

        /// <summary>
        /// 外部参照用ローリング無敵状態UniRx
        /// </summary>
        public IObservable<bool> IsRollingInvincibleObservable => isRollingInvincible;

        /// <summary>
        /// ローリング無敵状態有効時間
        /// </summary>
        private float isRollingInvincibleTime = 0.5f;

        private IDisposable rollingSubscription;

        private ISubscriber<BombActiveSignal> _bombActiveSubscriber;

        public void Setup(ISubscriber<BombActiveSignal> bombActiveSubscriber)
        {
            bombActiveSubscriber.Subscribe(signal =>
            {
                switch (signal.BombActiveType)
                {
                    case BombActiveType.Active:
                        // ボムがアクティブになった時にプレイヤーを無敵状態にする
                        isEventInvincible = true;
                        break;
                    case BombActiveType.Inactive:
                        // ボムの効果が終了した時に無敵状態を解除する
                        isEventInvincible = false;
                        break;
                }
            }).AddTo(this);
        }

        private void Start()
        {
            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Gamestart)
                .Subscribe(_ => isEventInvisibleActivate())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage1)
                .Subscribe(_ => isEventInvisibleDeActivate())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage2)
                .Subscribe(_ => isEventInvisibleDeActivate())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage3)
                .Subscribe(_ => isEventInvisibleDeActivate())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage2Interval)
                .Subscribe(_ => isEventInvisibleActivate())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Stage3Interval)
                .Subscribe(_ => isEventInvisibleActivate())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.GameClear)
                .Subscribe(_ => isEventInvisibleActivate())
                .AddTo(this);

            _gameStatus.CurrentGameStateReactiveProperty
                .Where(x => x == GameState.Gameover)
                .Subscribe(_ => isEventInvisibleActivate())
                .AddTo(this);

            MessageBroker.Default.Receive<PlayerShieldRecover>().Where(x => x.ShieldRecoverPoint > 0)
                .Subscribe(x => ShieldRecover(x.ShieldRecoverPoint)).AddTo(this);

            // ローリングシグナルを購読し、無敵時間を適用
            rollingSubscription = MessageBroker.Default.Receive<RollingSignal>()
                .Subscribe(_ => ApplyRollingInvincibility().Forget())
                .AddTo(this);
        }

        public void ShieldReduction(float damage)
        {
            var _damageExplosion = Instantiate(explosionDamage, transform.position, transform.rotation);

            shield.Value -= damage;
            DamageInvincible(this.GetCancellationTokenOnDestroy()).Forget();

            if (shield.Value < 0)
            {
                shield.Value = 0;
            }

            if (shield.Value == 0)
            {
                FuselageDestruction();
            }
        }

        public void FuselageDestruction()
        {
            MessageBroker.Default.Publish(new Gameover());

            var _destructionExplosion = Instantiate(explosionDestruction, transform.position, transform.rotation);
            Destroy(player);
        }

        /// <summary>
        /// 接触ダメージ処理の発火イベント
        /// </summary>
        /// <param name="collision">接触したゲームオブジェクト</param>
        private void OnCollisionStay(Collision collision)
        {
            if (InvisibleCheck())
            {
                return;
            }

            if (collision.gameObject.TryGetComponent(out IDamageableToPlayer damagetarget))
            {
                ShieldReduction(damagetarget.AddDamage());
            }
        }

        /// <summary>
        /// 貫通ダメージ処理の発火イベント
        /// </summary>
        /// <param name="other">貫通したゲームオブジェクト</param>
        private void OnTriggerEnter(Collider other)
        {
            if (InvisibleCheck())
            {
                return;
            }

            if (other.gameObject.TryGetComponent(out IDamageableToPlayer damagetarget))
            {
                ShieldReduction(damagetarget.AddDamage());
            }
        }

        /// <summary>
        /// 無敵状態のチェック
        /// </summary>
        /// <returns>無敵の有無のbool値</returns>
        private bool InvisibleCheck()
        {
            if (isEventInvincible == true)
            {
                return true;
            }

            if (isDamageInvincible.Value)
            {
                return true;
            }

            if (isRollingInvincible.Value)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// イベント無敵状態有効処理
        /// </summary>
        private void isEventInvisibleActivate()
        {
            isEventInvincible = true;
        }

        /// <summary>
        /// イベント無敵状態無効処理
        /// </summary>
        private void isEventInvisibleDeActivate()
        {
            isEventInvincible = false;
        }

        /// <summary>
        /// 被ダメージ無敵状態有効処理
        /// </summary>
        private async UniTaskVoid DamageInvincible(CancellationToken cancellationToken)
        {
            isDamageInvincible.Value = true;
            await UniTask.Delay(TimeSpan.FromSeconds(isDamageInvincibleTime), false, 0, cancellationToken);
            isDamageInvincible.Value = false;
        }

        /// <summary>
        /// ローリング無敵状態有効処理
        /// </summary>
        private async UniTaskVoid ApplyRollingInvincibility()
        {
            Debug.Log("ApplyRollingInvincibility");
            isRollingInvincible.Value = true;
            await UniTask.Delay(TimeSpan.FromSeconds(isRollingInvincibleTime));
            isRollingInvincible.Value = false;
        }

        /// <summary>
        /// シールド回復処理
        /// </summary>
        /// <param name="recover">シールド回復値</param>
        private void ShieldRecover(float recover)
        {
            shield.Value += recover;

            if (maxShield.Value < shield.Value)
            {
                shield.Value = maxShield.Value;
            }
        }

        /// <summary>
        /// シールド値が最大かどうか
        /// </summary>
        /// <returns></returns>
        public bool IsShieldFull()
        {
            return shield.Value == maxShield.Value;
        }

        /// <summary>
        /// シールド値が危険水準かどうか
        /// </summary>
        public bool IsShieldDanger()
        {
            return shield.Value < shieldDangerValue;
        }
    }
}
