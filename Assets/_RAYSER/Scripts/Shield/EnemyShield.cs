using Cysharp.Threading.Tasks;
using Damage;
using Status;
using Target;
using Turret;
using UnityEngine;

namespace Shield
{
    public class EnemyShield : MonoBehaviour, IShield, IDamageableToPlayer, IHitByLaser, ITargetedObject
    {
        /// <summary>
        /// 接触時ダメージ
        /// </summary>
        [SerializeField] private float damage;

        /// <summary>
        /// ゲームステータス
        /// </summary>
        [SerializeField] private GameStatus _gameStatus;

        /// <summary>
        /// 爆発処理のゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject explosionDestruction;

        /// <summary>
        /// ダメージ処理のゲームオブジェクト
        /// </summary>
        [SerializeField] private GameObject explosionDamage;

        [SerializeField] private float shield = 1;

        public void ShieldReduction(float damage)
        {
            // ダメージエフェクトをコメントアウト
            // var _damageExplosion = Instantiate(explosionDamage, transform.position, transform.rotation);

            shield -= damage;

            if (shield < 0)
            {
                shield = 0;
            }

            if (shield == 0)
            {
                FuselageDestruction();
            }
        }

        public void FuselageDestruction()
        {
            var _destructionExplosion = Instantiate(explosionDestruction, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        public float AddDamage()
        {
            return damage;
        }

        public void GetHitLaser(Laser _laser)
        {
            var damage = _laser.AddDamage();
            ShieldReduction(damage);
        }
    }
}
