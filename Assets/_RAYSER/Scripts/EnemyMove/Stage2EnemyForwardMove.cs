using Target;
using UnityEngine;

namespace EnemyMove
{
    using UniRx;
    using UniRx.Triggers;

    /// <summary>
    /// ステージ２敵機前進処理
    /// </summary>
    public class Stage2EnemyForwardMove : MonoBehaviour
    {
        [SerializeField] private EnemyTarget _enemyTarget;
        [SerializeField] private float moveSpeed = 0.1f;

        private void Start()
        {
            this.FixedUpdateAsObservable()
                .First(l => _enemyTarget.CurrentTarget() != null)
                .Subscribe(l => RotateForTarget())
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => _enemyTarget.CurrentTarget() != null)
                .Subscribe(l => ForwardForTarget())
                .AddTo(this);
        }

        /// <summary>
        /// ターゲットを向く
        /// </summary>
        private void RotateForTarget()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(_enemyTarget.CurrentTarget().transform.position - transform.position),
                0.3f);
            transform.LookAt(_enemyTarget.CurrentTarget().transform.position);
        }

        /// <summary>
        /// ターゲットを向き前進
        /// </summary>
        private void ForwardForTarget()
        {
            transform.position += transform.forward * moveSpeed;
        }
    }
}
