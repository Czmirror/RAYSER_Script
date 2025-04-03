using System.Collections.Generic;
using _RAYSER.Scripts.Turret.FirePointProvider;
using _RAYSER.Scripts.Turret.ShootingEffect;
using _RAYSER.Scripts.Turret.Trigger;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turret
{
    /// <summary>
    /// 敵機の射撃処理
    /// </summary>
    public class EnemyTurret : MonoBehaviour, ITurret
    {
        /// <summary>
        /// ショットインターバル
        /// </summary>
        [FormerlySerializedAs("shotInterbalTime")] [SerializeField] private float shotInterval = 1.5f;
        public float ShotInterval => shotInterval;

        /// <summary>
        /// 敵機のビームのPrefab
        /// </summary>
        [SerializeField] private EnemyBeam enemyBeamPrefab;

        /// <summary>
        /// 発射点のリスト
        /// </summary>
        [SerializeField] private List<FirePoint> firePoints = new List<FirePoint>();

        /// <summary>
        /// 発射トリガー
        /// </summary>
        [SerializeReference] public ITurretTrigger _trigger;

        /// <summary>
        /// 弾座標プロバイダー
        /// </summary>
        [SerializeReference] public IFirePointProvider _firePointProvider;

        [SerializeReference] public IShootingEffect _shootingEffect;

        private TurretShooter _turretShooter;
        private BeamPool _beamPool;

        public void Initialize()
        {
            if (_firePointProvider == null)
            {
                _firePointProvider = new FirePointProvider();
            }
            _firePointProvider.Initialize(transform, firePoints);

            _beamPool = new BeamPool(enemyBeamPrefab);
            _turretShooter = new TurretShooter(_firePointProvider, _beamPool, shotInterval);

            _trigger?.Initialize(this);

            _shootingEffect?.Initialize(transform);
        }

        public async UniTask StartShootingAsync()
        {
            // ゲームオブジェクトが有効である場合のみ発射
            if (gameObject.activeInHierarchy)
            {
                _turretShooter.StartShooting();
            }

            if (_shootingEffect != null)
            {
                // エフェクトを再生（非同期再生のためForget使用）
                _shootingEffect.PlayEffectAsync(shotInterval).Forget();
            }
        }

        public void StopShooting()
        {
            _shootingEffect?.StopEffect();
            _turretShooter.StopShooting();
        }

        public async UniTask CleanupAsync()
        {
            _turretShooter.Cleanup();
            if (_beamPool != null)
            {
                await _beamPool.ClearPoolAsync();
            }
        }

        private void OnEnable()
        {
            Initialize();
        }

        private async void OnDisable()
        {
            StopShooting();
            await CleanupAsync();
        }

        private async void OnDestroy()
        {
            await CleanupAsync();
        }
    }
}
