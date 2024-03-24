using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// バルカン : サブウェポンの一種 Visitor
    /// </summary>
    [System.Serializable]
    public class Vulcan : ISubWeaponVisitor
    {
        [SerializeField] private VulkanBullet bulletPrefab;
        private float firingInterval = 0.1f;
        private float lastFireTime = 0f;

        public void Visit(FireAction action)
        {
            if (Time.time < lastFireTime + firingInterval)
            {
                return;
            }

            GameObject.Instantiate(bulletPrefab, action.Position, action.Rotation);
            lastFireTime = Time.time;
        }

        public void Reset()
        {
            lastFireTime = 0f;
        }
    }
}
