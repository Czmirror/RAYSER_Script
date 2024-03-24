using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// MultiWay : サブウェポンの一種 Visitor
    /// </summary>
    [System.Serializable]
    public class ThreeWay : ISubWeaponVisitor
    {
        [SerializeField] private VulkanBullet bulletPrefab;
        private float firingInterval = 1f;
        private float lastFireTime = 0f;

        // 角度の配列
        private float[] angles = new float[] { -15f, 0f, 15f };

        public void Visit(FireAction action)
        {
            if (Time.time < lastFireTime + firingInterval)
            {
                return;
            }

            // 各角度で弾丸を発射
            foreach (float angleY in angles)
            {
                // Y軸での回転
                foreach (float angleZ in angles)
                {
                    // Z軸での回転
                    Quaternion rotation = Quaternion.Euler(0, angleY, angleZ) * action.Rotation;
                    GameObject.Instantiate(bulletPrefab, action.Position, rotation);
                }
            }

            lastFireTime = Time.time;
        }

        public void Reset()
        {
            lastFireTime = 0f;
        }
    }
}
