using Shield;
using UnityEngine;

namespace Target
{
    /// <summary>
    /// 敵機のロックオン処理
    /// </summary>
    public class EnemyLockOn : MonoBehaviour
    {
        /// <summary>
        /// ターゲット
        /// </summary>
        [SerializeField] private GameObject target = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.TryGetComponent(out PlayerShield player))
            {
                target = player.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.TryGetComponent(out PlayerShield player))
            {
                target = null;
            }
        }

        public GameObject CurrentTarget()
        {
            return target;
        }
    }
}
