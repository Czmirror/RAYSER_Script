using UnityEngine;

namespace BGM
{
    /// <summary>
    /// BGMループ再生クラス
    /// </summary>
    public class LoopSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        /// <summary>
        /// 再生中の音源
        /// </summary>
        private LoopSound loopSound;

        /// <summary>
        /// ループ開始地点のサンプル数
        /// </summary>
        private int loopStartSamples;

        /// <summary>
        /// ループ終了地点のサンプル数
        /// </summary>
        private int loopEndSamples;

        /// <summary>
        /// ループ時間に含まれるサンプル数
        /// </summary>
        private int loopLengthSamples;

        private void Update()
        {
            // ループ終了地点に達したら、ループ開始地点に戻す
            if (audioSource.timeSamples >= loopEndSamples)
            {
                audioSource.timeSamples -= loopLengthSamples;
                audioSource.Play();
            }
        }

        /// <summary>
        /// 外部から音源ScriptableObjectを切り替えるメソッド
        /// </summary>
        /// <param name="newLoopSound"></param>
        public void ChangeLoopSound(LoopSound newLoopSound)
        {
            // 新しい音源ScriptableObjectを代入する
            loopSound = newLoopSound;

            // AudioSourceにAudioClipを設定する
            audioSource.clip = loopSound.clip;

            // AudioClipとskipTimeからサンプル数を計算する
            loopStartSamples = (int)(loopSound.clip.frequency * loopSound.skipTime);
            loopEndSamples = loopSound.clip.samples;
            loopLengthSamples = loopEndSamples - loopStartSamples;

            // AudioSourceを再生する
            audioSource.Play();
        }

        /// <summary>
        /// 音源停止メソッド
        /// </summary>
        public void Stop()
        {
            audioSource.Stop();
        }
    }
}
