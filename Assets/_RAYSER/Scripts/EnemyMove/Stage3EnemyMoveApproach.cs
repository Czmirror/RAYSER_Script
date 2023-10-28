using Target;
using UnityEngine;

namespace EnemyMove
{
    public class Stage3EnemyMoveApproach : MonoBehaviour
    {
        [SerializeField] private EnemyTarget _enemyTarget;
        [SerializeField] private float moveSpeed = 0.75f;

        /// <summary>
        /// 移動可能制限値
        /// </summary>
        private float restrictedMovementX = -950f;

        /// <summary>
        /// ターゲットへの接近可能距離
        /// </summary>
        [SerializeField] private float stopDistance = 5f;

        void FixedUpdate()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(_enemyTarget.CurrentTarget().transform.position - transform.position), 0.3f);
            transform.LookAt(_enemyTarget.CurrentTarget().transform.position);

            // 接近可能範囲まで近づく
            var distance = Vector3.Distance(transform.position, _enemyTarget.CurrentTarget().transform.position);
            if (distance > stopDistance)
            {
                transform.position += transform.forward * moveSpeed;
                MovementRestrictions();
            }
        }

        public void MovementRestrictions()
        {
            if (transform.position.x <= restrictedMovementX)
            {
                transform.position = new Vector3(restrictedMovementX, transform.position.y, transform.position.z);
            }
        }
    }
}
