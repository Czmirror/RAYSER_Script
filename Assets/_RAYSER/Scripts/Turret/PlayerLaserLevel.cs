using System;
using Event.Signal;
using UniRx;
using UnityEngine;

namespace Turret
{
    /// <summary>
    /// 自機のレーザーレベル
    /// </summary>
    public class PlayerLaserLevel : MonoBehaviour
    {
        /// <summary>
        /// レーザーレベルUniRx
        /// </summary>
        [SerializeField] private ReactiveProperty<int> currentLaserLevel = new ReactiveProperty<int>(1);

        /// <summary>
        /// 外部参照用レーザーレベルUniRx
        /// </summary>
        public IObservable<int> CurrentLaserLevelObservable => currentLaserLevel;

        /// <summary>
        /// 最大レーザーレベル
        /// </summary>
        [SerializeField] private int maxLaserLevel = 3;

        /// <summary>
        /// レベル１レーザーのPrefab
        /// </summary>
        [SerializeField] private Laser level1Laser;

        /// <summary>
        /// レベル２レーザーのPrefab
        /// </summary>
        [SerializeField] private Laser level2Laser;

        /// <summary>
        /// レベル３レーザーのPrefab
        /// </summary>
        [SerializeField] private Laser level3Laser;

        private void Start()
        {
            MessageBroker.Default.Receive<PlayerLaserLevelUp>().Subscribe(_ => LaserLevelUp()).AddTo(this);
        }

        public Laser CurrentLaser()
        {
            switch (currentLaserLevel.Value)
            {
                case 1:
                    return level1Laser;
                case 2:
                    return level2Laser;
                case 3:
                    return level3Laser;
                default:
                    return level1Laser;
            }
        }

        /// <summary>
        /// レーザーレベルアップ
        /// </summary>
        public void LaserLevelUp()
        {
            if (currentLaserLevel.Value == maxLaserLevel)
            {
                return;
            }

            currentLaserLevel.Value += 1;
        }
    }
}
