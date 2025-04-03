using Damage;
using UnityEngine;

namespace _RAYSER.Scripts.SubWeapon
{
    public class SpiralBullet: MonoBehaviour, IDamageableToEnemy
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float beamSpeed = 30f; // 前進スピード
        [SerializeField] private float spiralSpeed = 5f; // 螺旋の速度
        [SerializeField] private float spiralRadius = 1f; // 螺旋の半径
        [SerializeField] private float disappearTime = 5f; // 消滅までの時間
        [SerializeField] private float damage = 1f;

        private float spiralAngle = 0f;

        private void Start()
        {
            Destroy(gameObject, disappearTime);
        }

        private void Update()
        {
            // 螺旋角度を更新
            spiralAngle += spiralSpeed * Time.deltaTime;

            // 螺旋のXとY座標を計算
            float x = Mathf.Cos(spiralAngle) * spiralRadius;
            float y = Mathf.Sin(spiralAngle) * spiralRadius;

            // 螺旋の移動方向を設定
            Vector3 spiralMovement = new Vector3(x, y, beamSpeed);
            _rigidbody.linearVelocity = transform.TransformDirection(spiralMovement);
        }

        public float AddDamage()
        {
            return damage;
        }
    }
}
