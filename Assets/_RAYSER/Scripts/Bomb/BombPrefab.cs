using Damage;
using UnityEngine;

namespace _RAYSER.Scripts.Bomb
{
    /// <summary>
    /// ボムのPrefab
    /// </summary>
    public class BombPrefab : MonoBehaviour, IDamageableToEnemy
    {
        [SerializeField] private Rigidbody _rigidbody;

        /// <summary>
        /// 接触時ダメージ
        /// </summary>
        [SerializeField] private float damage;

        public float AddDamage()
        {
            return damage;
        }
    }
}
