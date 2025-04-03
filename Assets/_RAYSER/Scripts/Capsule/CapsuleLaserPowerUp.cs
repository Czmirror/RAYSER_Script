using Event.Signal;
using Status;
using UniRx;
using UnityEngine;

namespace Capsule
{
    /// <summary>
    /// レーザーレベルアップカプセル処理
    /// </summary>
    public class CapsuleLaserPowerUp : MonoBehaviour, ICapsuleinfo
    {
        [SerializeField] private string _name = "LaserPowerUp";
        public string Name => _name;

        [SerializeField] private CapsuleEnum capsuleEnum = CapsuleEnum.LaserPowerUp;
        [SerializeField] private AudioSource _audioSource;

        /// <summary>
        /// カプセル取得時のサウンド
        /// </summary>
        [SerializeField] private AudioClip capsuleRecoverySound;

        /// <summary>
        /// カプセル発動時のサウンド
        /// </summary>
        [SerializeField] private AudioClip capsuleEffectActivatedSound;

        public void CapsuleRecovery()
        {
            // _audioSource.clip = capsuleRecoverySound;
            // _audioSource.Play();
            MessageBroker.Default.Publish(new PlayerGetCapsule(this));
            Destroy(gameObject);
        }

        public void CapsuleEffectActivated()
        {
            // _audioSource.clip = capsuleEffectActivatedSound;
            // _audioSource.Play();
            MessageBroker.Default.Publish(new PlayerLaserLevelUp());
        }

        public CapsuleEnum GetCapsuleEnum()
        {
            return capsuleEnum;
        }
    }
}
