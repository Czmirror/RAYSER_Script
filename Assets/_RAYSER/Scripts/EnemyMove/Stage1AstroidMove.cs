using UnityEngine;

namespace EnemyMove
{
    public class Stage1AstroidMove : MonoBehaviour
    {
        private float moveSpeed = 0.1f;

        /// <summary>
        /// 目標ゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject target;

        /// <summary>
        /// 回転速度
        /// </summary>
        // [SerializeField] private float tumble = 0.25f;

        public void Move()
        {
            if (!target) return;

            transform.position += transform.forward * moveSpeed;
        }

        private void Start()
        {
            if (target == null)
            {
                target = GameObject.Find("RAYSER");
            }

            var targetPosition = target.transform.position;
            transform.LookAt(targetPosition);

            // TODO 回転は別スクリプト（違う方向へ飛んでしまうため）
            // GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;

        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}
