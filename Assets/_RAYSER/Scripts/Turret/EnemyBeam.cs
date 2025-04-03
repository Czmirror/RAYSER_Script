using System;
using Damage;
using UnityEngine;

namespace Turret
{
    /// <summary>
    /// 敵機ビーム用クラス
    /// </summary>
    public class EnemyBeam : MonoBehaviour, IDamageableToPlayer
    {
        public System.Action OnDeactivation;

        [SerializeField] private Rigidbody _rigidbody;

        /// <summary>
        /// ビームのスピード
        /// </summary>
        [SerializeField] private float beamSpeed = 10f;

        /// <summary>
        /// ダメージ量
        /// </summary>
        [SerializeField] private float damage = 3f;

        /// <summary>
        /// 自動消滅時間
        /// </summary>
        [SerializeField] private float disappearTime = 5f;

        private void OnEnable()
        {
            Invoke(nameof(Deactivate), 5f);
        }

        public float AddDamage()
        {
            return damage;
        }

        private void Update(){
            _rigidbody.linearVelocity = transform.forward.normalized * beamSpeed;
        }

        private void Deactivate()
        {
            // 非アクティブ化処理
            if (OnDeactivation != null)
            {
                // イベントを発火
                OnDeactivation.Invoke();

                // イベントのクリア
                OnDeactivation = null;
            }

            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke(); // 非アクティブ時にタイマーを解除
        }
    }
}
