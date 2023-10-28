using Target;
using UnityEngine;

namespace EnemyMove
{
    /// <summary>
    /// ステージ１敵機追跡処理
    /// </summary>
    public class Stage1EnemyMoveChase : MonoBehaviour
    {
        /// <summary>
        /// 敵機のターゲット処理
        /// </summary>
        [SerializeField] private EnemyTarget _enemyTarget;

        /// <summary>
        /// 敵機の移動速度
        /// </summary>
        [SerializeField] private float moveSpeed = 0.1f;

        /// <summary>
        /// ターゲットへの接近可能距離
        /// </summary>
        [SerializeField] private float stopDistance = 10f;

        void FixedUpdate()
        {
            if (_enemyTarget.CurrentTarget() == null)
            {
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(_enemyTarget.CurrentTarget().transform.position - transform.position), 0.3f);

            var distance = Vector3.Distance(transform.position, _enemyTarget.CurrentTarget().transform.position);

            if (distance < stopDistance)
            {
                return;
            }

            transform.position += transform.forward * moveSpeed;
        }
    }
}
