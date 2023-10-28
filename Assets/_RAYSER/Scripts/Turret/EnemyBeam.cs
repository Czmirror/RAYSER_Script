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

        /// <summary>
        /// ビームの大元のゲームオブジェクト
        /// </summary>
        private GameObject rootObject;
        public float AddDamage()
        {
            return damage;
        }

        private void Start()
        {
            // _rigidbody.velocity = transform.forward.normalized * beamSpeed;
            rootObject = transform.root.gameObject;
            Destroy(rootObject, disappearTime);
        }

        private void Update(){
            _rigidbody.velocity = transform.forward.normalized * beamSpeed;
            // transform.Translate(Vector3.forward * beamSpeed * Time.deltaTime);
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.gameObject.TryGetComponent(out IDamageableToEnemy damagetarget))
        //     {
        //         Destroy(rootObject);
        //     }
        // }
    }
}
