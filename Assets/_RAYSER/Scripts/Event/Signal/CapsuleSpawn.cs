using UnityEngine;

namespace Event.Signal
{
    /// <summary>
    /// カプセル発生通知クラス
    /// </summary>
    public class CapsuleSpawn
    {
        /// <summary>
        /// カプセル発生地点
        /// </summary>
        public readonly Transform CapsuleSpawnPoint;

        public CapsuleSpawn(Transform capsuleSpawnTransform)
        {
            CapsuleSpawnPoint = capsuleSpawnTransform;
        }
    }
}
