using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// ホーミング : サブウェポンの一種 Visitor
    /// </summary>
    [System.Serializable]
    public class Homing : ISubWeaponVisitor
    {
        [SerializeField] private HomingBullet bulletPrefab;
        private float firingInterval = 0.5f;
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
