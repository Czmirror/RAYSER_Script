using Target;
using UnityEngine;

namespace EnemyMove
{
    /// <summary>
    /// ステージ2敵機追跡処理
    /// </summary>
    public class Stage2EnemyMoveChase : MonoBehaviour
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

        /// <summary>
        /// 敵機の制限移動X軸最大範囲
        /// </summary>
        private float maxXPosition = -810;

        /// <summary>
        /// 敵機の制限移動Y軸最大範囲
        /// </summary>
        private float maxYPosition = 5;

        /// <summary>
        /// 敵機の制限移動Y軸最小範囲
        /// </summary>
        private float minYPosition = -5;

        /// <summary>
        /// 敵機のZ軸制限移動Z軸最小範囲
        /// </summary>
        private float maxZPosition = 0;

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

            // 制限範囲外に移動しないように制限をかける
            if (transform.position.x > maxXPosition)
            {
                transform.position = new Vector3(maxXPosition, transform.position.y, transform.position.z);
            }

            if (transform.position.y < minYPosition)
            {
                transform.position = new Vector3(transform.position.x, minYPosition, transform.position.z);
            }

            if (transform.position.y > maxYPosition)
            {
                transform.position = new Vector3(transform.position.x, maxYPosition, transform.position.z);
            }

            if (transform.position.z > maxZPosition)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, maxZPosition);
            }
        }
    }
}
