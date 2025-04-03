using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UniRx;

namespace Turret
{
    /// <summary>
    /// 砲台の制御クラス
    /// </summary>
    public class TurretController
    {
        private readonly GameObject owner;
        private readonly EnemyBeam enemyBeamPrefab;
        private readonly float shotInterval;
        private ObjectPool<EnemyBeam> beamPool;
        private List<EnemyBeam> activeBeams = new List<EnemyBeam>();
        private IDisposable shotSubscription;
        private readonly Func<List<Vector3>> getPosition;
        private readonly Func<List<Quaternion>> getRotation;

        public TurretController(
            GameObject owner,
            EnemyBeam enemyBeamPrefab,
            float shotInterval,
            Func<List<Vector3>> getPosition,
            Func<List<Quaternion>> getRotation
            )
        {
            this.owner = owner;
            this.enemyBeamPrefab = enemyBeamPrefab;
            this.shotInterval = shotInterval;
            this.getPosition = getPosition;
            this.getRotation = getRotation;

            // その他の初期化
            InitializePool();
        }

        private void InitializePool()
        {
            beamPool = new ObjectPool<EnemyBeam>(
                createFunc: () => GameObject.Instantiate(enemyBeamPrefab),
                actionOnGet: beam => beam.gameObject.SetActive(true),
                actionOnRelease: beam => beam.gameObject.SetActive(false),
                actionOnDestroy: beam => GameObject.Destroy(beam.gameObject),
                defaultCapacity: 10,
                maxSize: 20
            );
        }

        public void StartShooting()
        {
            StopShooting(); // 既存の購読を停止して重複を防止

            shotSubscription = Observable
                .Interval(TimeSpan.FromSeconds(shotInterval))
                .Subscribe(_ => Shoot())
                .AddTo(owner);
        }

        public void StopShooting()
        {
            shotSubscription?.Dispose();
        }

        private void Shoot()
        {
            if (!owner.transform.root.gameObject.activeSelf) return;

            // 発射点のリストを取得
            var positions = getPosition();
            var rotations = getRotation();

            // 発射点と回転の数が一致していない場合はエラーを表示
            if (positions.Count != rotations.Count)
            {
                Debug.LogError("Positions and Rotations count mismatch.");
                return;
            }

            // 各発射点に基づいてビームを発射
            for (int i = 0; i < positions.Count; i++)
            {
                var beam = beamPool.Get();
                beam.transform.position = positions[i];
                beam.transform.rotation = rotations[i];

                activeBeams.Add(beam);

                // ビームが非アクティブ化された際にプールに戻す処理
                Action releaseAction = null;
                releaseAction = () =>
                {
                    beamPool.Release(beam);
                    beam.OnDeactivation -= releaseAction;
                    activeBeams.Remove(beam);
                };

                beam.OnDeactivation += releaseAction;
            }
        }

        public async void Cleanup()
        {
            StopShooting();

            await UniTask.Delay(5000);

            foreach (var beam in activeBeams)
            {
                if (beam != null)
                {
                    GameObject.Destroy(beam.gameObject);
                }
            }

            activeBeams.Clear();
            beamPool?.Clear();
        }
    }
}
