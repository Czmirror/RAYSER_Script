using Event;
using UnityEngine;

namespace Target
{
    /// <summary>
    /// 自機のターゲッティング処理
    /// </summary>
    public class PlayerTargeting : MonoBehaviour
    {
        /// <summary>
        /// ターゲット対象の敵機
        /// </summary>
        [SerializeField] private GameObject targetEnemy;

        /// <summary>
        /// ターゲットが存在しない場合の照準
        /// </summary>
        [SerializeField] private GameObject unTargetedObjects;

        /// <summary>
        /// 現在設定されているターゲットのゲームオブジェクトを返却
        /// </summary>
        /// <returns>ターゲットのゲームオブジェクト</returns>
        public GameObject CurrentTargetGameObject()
        {
            if (IsTarget())
            {
                return targetEnemy;
            }

            return unTargetedObjects;
        }

        /// <summary>
        /// ターゲティング中を判定する
        /// </summary>
        /// <returns>ロックオン中を表すbool値</returns>
        public bool IsTarget()
        {
            if (targetEnemy != null)
            {
                return true;
            }

            return false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<ITargetedObject>() is var targetedObject && targetedObject != null)
            {
                targetEnemy = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            targetEnemy = null;
        }
    }
}
