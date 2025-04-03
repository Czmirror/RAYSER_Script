using System.Collections.Generic;
using Event.Signal;
using UI.Game;
using UniRx;
using UnityEngine;

namespace Turret
{
    public class Stage2BossTurret : MonoBehaviour
    {
        /// <summary>
        /// ショットの自動消滅時間
        /// </summary>
        [SerializeField] private float shotInterbalTime = 1.5f;

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
            _turretController = new TurretController(
                owner: gameObject,
                enemyBeamPrefab: enemyBeamPrefab,
                shotInterval: shotInterbalTime,
                getPosition: () => firePoints.ConvertAll(fp => transform.TransformPoint(fp.Position)),
                getRotation: () => firePoints.ConvertAll(fp => transform.rotation * Quaternion.Euler(fp.Rotation))
            );

            MessageBroker.Default.Receive<Stage2BossEncounter>()
                .Where(x => x._talk == TalkEnum.TalkEnd)
                .Subscribe(_ => _turretController.StartShooting())
                .AddTo(this);
        }

        private void OnDestroy()
        {
            _turretController?.Cleanup();
        }
    }
}
