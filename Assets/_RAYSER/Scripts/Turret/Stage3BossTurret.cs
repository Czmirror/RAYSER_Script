using System.Collections.Generic;
using Event.Signal;
using Target;
using UniRx;
using UnityEngine;

namespace Turret
{
    public class Stage3BossTurret : MonoBehaviour
    {
        [SerializeField] private EnemyTarget _enemyTarget;

        /// <summary>
        /// 標的となる自機
        /// </summary>
        [SerializeField] private GameObject targetPlayer;

        /// <summary>
        /// ショットの自動消滅時間
        /// </summary>
        [SerializeField] private float shotInterbalTime = 0.75f;

        /// <summary>
        /// 敵機のビームのゲームオブジェクト
        /// </summary>
        [SerializeField] private EnemyBeam enemyBeamPrefab;

        /// <summary>
        /// 発射点のリスト
        /// </summary>
        [SerializeField] private List<FirePoint> firePoints = new List<FirePoint>();

        private TurretController _turretController;

        private void Start()
        {
            _enemyTarget.TargetInitialize(targetPlayer);

            _turretController = new TurretController(
                owner: gameObject,
                enemyBeamPrefab: enemyBeamPrefab,
                shotInterval: shotInterbalTime,
                getPosition: () => firePoints.ConvertAll(fp => transform.TransformPoint(fp.Position)),
                getRotation: () =>
                {
                    var currentTarget = _enemyTarget.CurrentTarget();
                    if (currentTarget != null)
                    {
                        return firePoints.ConvertAll(fp =>
                            Quaternion.LookRotation(currentTarget.transform.position - transform.TransformPoint(fp.Position))
                        );
                    }
                    return firePoints.ConvertAll(fp => transform.rotation * Quaternion.Euler(fp.Rotation));
                }
            );

            // メッセージ受信で発射開始
            MessageBroker.Default.Receive<Stage3Start>()
                .Subscribe(_ => _turretController.StartShooting())
                .AddTo(this);

            // メッセージ受信で発射停止
            MessageBroker.Default.Receive<GameClear>()
                .Subscribe(_ =>
                {
                    _turretController.StopShooting();
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            // TurretControllerのリソースを解放
            _turretController?.Cleanup();
        }
    }
}
