using System;
using UniRx;
using UnityEngine;

namespace Shield
{
    /// <summary>
    /// シールドエフェクト表示用クラス
    /// </summary>
    public class PlayerShieldEffect : MonoBehaviour
    {
        [SerializeField] private PlayerShield _playerShield;
        [SerializeField] private ParticleSystem _particleSystem;

        private void Start()
        {
            _particleSystem.Stop();
            _playerShield.IsDamageInvincibleObservable.Where(x => x ).Subscribe(_ => ShieldEffectActive()).AddTo(this);
            _playerShield.IsDamageInvincibleObservable.Where(x => x == false).Subscribe(_ => ShieldEffectDeactive()).AddTo(this);

        }

        /// <summary>
        /// シールドエフェクト有効
        /// </summary>
        private void ShieldEffectActive()
        {
            _particleSystem.Play();
        }

        /// <summary>
        /// シールドエフェクト
        /// </summary>
        private void ShieldEffectDeactive()
        {
            _particleSystem.Stop();
        }
    }
}
