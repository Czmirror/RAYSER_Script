using UnityEngine;

namespace Turret
{
    /// <summary>
    /// レーザーのヒットエフェクト消滅処理
    /// </summary>
    public class LaserHitEffectExtinguishment : MonoBehaviour
    {
        /// <summary>
        /// 消滅時間
        /// </summary>
        [SerializeField] float extinguishingTime = 0.1f;

        public void Start()
        {
            Destroy (gameObject, extinguishingTime);
        }
    }
}
