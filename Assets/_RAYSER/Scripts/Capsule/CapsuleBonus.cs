using Event.Signal;
using Status;
using UniRx;
using UnityEngine;

namespace Capsule
{
    /// <summary>
    /// ボーナスカプセル処理
    /// </summary>
    public class CapsuleBonus : MonoBehaviour, ICapsuleinfo
    {
        [SerializeField] private string _name = "BonusScore";
        public string Name => _name;

        [SerializeField] private CapsuleEnum capsuleEnum = CapsuleEnum.BonusScore;

        [SerializeField] private AudioSource _audioSource;

        /// <summary>
        /// カプセル取得時のサウンド
        /// </summary>
        [SerializeField] private AudioClip capsuleRecoverySound;

        /// <summary>
        /// カプセル発動時のサウンド
        /// </summary>
        [SerializeField] private AudioClip capsuleEffectActivatedSound;

        /// <summary>
        /// 加算スコア
        /// </summary>
        [SerializeField] private int score = 10000;
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
            MessageBroker.Default.Publish(new ScoreAccumulation{Score = score});
        }

        public CapsuleEnum GetCapsuleEnum()
        {
            return capsuleEnum;
        }
    }
}
