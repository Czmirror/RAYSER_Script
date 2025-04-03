using Event.Signal;
using Status;
using UniRx;
using UnityEngine;

namespace Capsule
{
    /// <summary>
    /// スピードアップカプセル処理
    /// </summary>
    public class CapsuleSpeedUp : MonoBehaviour, ICapsuleinfo
    {
        [SerializeField] private  string _name = "SpeedUp";
        public string Name => _name;
        [SerializeField] private CapsuleEnum capsuleEnum = CapsuleEnum.SpeedUp;
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
            MessageBroker.Default.Publish(new PlayerMoveSpeedLevelUp());
        }

        public CapsuleEnum GetCapsuleEnum()
        {
            return capsuleEnum;
        }
    }
}
