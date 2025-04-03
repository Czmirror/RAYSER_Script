using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Turret
{
    /// <summary>
    /// ビームのオブジェクトプール管理クラス
    /// </summary>
    public class BeamPool
    {
        private readonly ObjectPool<EnemyBeam> beamPool;
        private readonly List<EnemyBeam> activeBeams = new List<EnemyBeam>();

        private int _cleanUpDelay = 5000;

        public BeamPool(EnemyBeam beamPrefab)
        {
            beamPool = new ObjectPool<EnemyBeam>(
                createFunc: () =>
                {
                    var beam = Object.Instantiate(beamPrefab);
                    beam.OnDeactivation += () => ReleaseBeam(beam); // 非アクティブ化時にプールへ戻す
                    return beam;
                },
                actionOnGet: beam =>
                {
                    beam.gameObject.SetActive(true);
                    activeBeams.Add(beam);
                },
                actionOnRelease: beam =>
                {
                    beam.gameObject.SetActive(false);
                    activeBeams.Remove(beam);
                },
                actionOnDestroy: beam => Object.Destroy(beam.gameObject),
                defaultCapacity: 10,
                maxSize: 50
            );
        }

        public EnemyBeam GetBeam()
        {
            return beamPool.Get();
        }

        private void ReleaseBeam(EnemyBeam beam)
        {
            beamPool.Release(beam);
        }

        public async UniTask ClearPoolAsync()
        {
            await UniTask.Delay(_cleanUpDelay);

            // すべてのアクティブなビームを強制的に非アクティブ化
            foreach (var beam in new List<EnemyBeam>(activeBeams))
            {
                ReleaseBeam(beam);
            }
            activeBeams.Clear();

            // プールそのものもクリア
            beamPool.Clear();
        }
    }
}
