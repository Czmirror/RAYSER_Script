using Damage;
using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// バルカンの弾
    /// </summary>
    public class VulkanBullet : MonoBehaviour, IDamageableToEnemy
    {
        [SerializeField] private Rigidbody _rigidbody;

        /// <summary>
        /// 弾のスピード
        /// </summary>
        [SerializeField] private float beamSpeed = 30f;

        /// <summary>
        /// ダメージ量
        /// </summary>
        [SerializeField] private float damage = 1f;

        /// <summary>
        /// 自動消滅時間
        /// </summary>
        [SerializeField] private float disappearTime = 5f;

        /// <summary>
        /// 弾の大元のゲームオブジェクト
        /// </summary>
        private GameObject rootObject;
        public float AddDamage()
        {
            return damage;
        }

        private void Start()
        {
            rootObject = transform.root.gameObject;
            Destroy(rootObject, disappearTime);
        }

        private void Update(){
            _rigidbody.velocity = transform.forward.normalized * beamSpeed;
        }
    }
}
