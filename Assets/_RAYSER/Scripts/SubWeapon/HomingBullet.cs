using Damage;
using Shield;
using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// ホーミング弾
    /// </summary>
    public class HomingBullet: MonoBehaviour, IDamageableToEnemy
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float beamSpeed = 30f;
        [SerializeField] private float damage = 1f;
        [SerializeField] private float disappearTime = 5f;

        /// <summary>
        /// ホーミングの敏感さ
        /// </summary>
        [SerializeField] private float homingSensitivity = 10f;

        private GameObject rootObject;
        /// <summary>
        /// ターゲットとなる敵機のTransform
        /// </summary>
        private Transform target;

        public float AddDamage()
        {
            return damage;
        }

        private void Start()
        {
            rootObject = transform.root.gameObject;
            Destroy(rootObject, disappearTime);
            FindClosestEnemy(); // 最も近い敵を見つける
        }

        private void Update()
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, direction * beamSpeed, homingSensitivity * Time.deltaTime);
            }
            else
            {
                _rigidbody.velocity = transform.forward * beamSpeed;
            }
        }

        private void FindClosestEnemy()
        {
            // すべてのEnemyShieldコンポーネントを持つオブジェクトを取得
            EnemyShield[] enemies = FindObjectsOfType<EnemyShield>();

            float closestDistance = float.MaxValue;
            Transform closestEnemy = null;

            foreach (EnemyShield enemy in enemies)
            {
                float distance = (enemy.transform.position - transform.position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }

            target = closestEnemy;
        }

    }
}
