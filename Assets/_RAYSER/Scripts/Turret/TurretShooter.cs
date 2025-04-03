using System;
using _RAYSER.Scripts.Turret.FirePointProvider;
using UniRx;

namespace Turret
{
    /// <summary>
    /// 砲撃処理クラス
    /// </summary>
    public class TurretShooter
    {
        private readonly IFirePointProvider firePointProvider;
        private readonly BeamPool beamPool;
        private readonly float shotInterval;
        private IDisposable shootingSubscription;

        public TurretShooter(IFirePointProvider firePointProvider, BeamPool beamPool, float shotInterval)
        {
            this.firePointProvider = firePointProvider;
            this.beamPool = beamPool;
            this.shotInterval = shotInterval;
        }

        public void StartShooting()
        {
            StopShooting();

            shootingSubscription = Observable.Interval(TimeSpan.FromSeconds(shotInterval))
                .Subscribe(_ => Shoot());
        }

        public void StopShooting()
        {
            shootingSubscription?.Dispose();
        }

        public void Cleanup()
        {
            StopShooting();
        }

        private void Shoot()
        {
            var positions = firePointProvider.GetPositions();
            var rotations = firePointProvider.GetRotations();

            for (int i = 0; i < positions.Count; i++)
            {
                var beam = beamPool.GetBeam();
                beam.transform.position = positions[i];
                beam.transform.rotation = rotations[i];
                beam.gameObject.SetActive(true);
            }
        }
    }
}
