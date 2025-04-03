using System.Collections.Generic;
using Event.Signal;
using Target;
using UniRx;
using UnityEngine;

namespace Turret
{
    /// <summary>
    /// ステージ2砲台弾発射クラス
    /// </summary>
    public class Stage2MainGunTurret: MonoBehaviour
    {
        /// <summary>
        /// ショットインターバル
        /// </summary>
        [SerializeField] private float shotInterbalTime = 1.0f;

        /// <summary>
        /// 敵機のビームのゲームオブジェクト
        /// </summary>
        [SerializeField] private EnemyBeam enemyBeamPrefab;

        private TurretController _turretController;

        [SerializeField] private GameObject target = null;

        [SerializeField] private EnemyLockOn _enemyLockOn;

        /// <summary>
        /// 発射点のリスト
        /// </summary>
        [SerializeField] private List<FirePoint> firePoints = new List<FirePoint>();

        private void Start()
        {
            _turretController = new TurretController(
                owner: gameObject,
                enemyBeamPrefab: enemyBeamPrefab,
                shotInterval: shotInterbalTime,
                getPosition: () => firePoints.ConvertAll(fp => transform.TransformPoint(fp.Position)),
                getRotation: () => firePoints.ConvertAll(fp => transform.rotation * Quaternion.Euler(fp.Rotation))
                );

            MessageBroker.Default.Receive<Stage2Start>()
                .Subscribe(_ => _turretController.StartShooting())
                .AddTo(this);
        }

        private void OnDestroy()
        {
            // TurretControllerのリソースを解放
            _turretController?.Cleanup();
        }
    }
}
