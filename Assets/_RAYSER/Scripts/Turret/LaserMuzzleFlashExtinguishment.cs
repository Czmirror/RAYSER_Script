using UnityEngine;

namespace Turret
{
    /// <summary>
    /// レーザーのマズルフラッシュ消滅処理
    /// </summary>
    public class LaserMuzzleFlashExtinguishment : MonoBehaviour
    {
        /// <summary>
        /// 消滅時間
        /// </summary>
        [SerializeField] float extinguishingTime = 0.3f;

        public void Start()
        {
            Destroy (gameObject, extinguishingTime);
        }
    }
}
