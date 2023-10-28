using Damage;
using Shield;
using UnityEngine;

namespace Burst
{
    /// <summary>
    /// 命中すると爆発する処理
    /// </summary>
    public class StrikeBurst : MonoBehaviour
    {
        [SerializeField] private EnemyShield _enemyShield;

        /// <summary>
        /// 接触ダメージ処理の発火イベント
        /// </summary>
        /// <param name="collision">接触したゲームオブジェクト</param>
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerShield damagetarget))
            {
                Burst();
            }
        }

        /// <summary>
        /// 貫通ダメージ処理の発火イベント
        /// </summary>
        /// <param name="other">貫通したゲームオブジェクト</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerShield damagetarget))
            {
                Burst();
            }
        }

        private void Burst()
        {
            _enemyShield.ShieldReduction(9999);
        }
    }
}
