using System;
using Event.Signal;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Capsule
{
    /// <summary>
    /// 敵機カプセルドロップ処理
    /// </summary>
    public class EnemyCapsuleDrop : MonoBehaviour
    {
        /// <summary>
        /// アイテム取得確率
        /// </summary>
        [SerializeField] private int itemOccurrenceProbability = 5;

        private void OnDestroy()
        {
            var isDropItemNumber = Random.Range(1, 100);

            if (isDropItemNumber >= itemOccurrenceProbability)
            {
                return;
            }

            MessageBroker.Default.Publish(new CapsuleSpawn(transform));
        }
    }
}
