using UnityEngine;
using VContainer;

namespace BGM.Volume
{
    /// <summary>
    /// 音量管理サービスクラス
    /// </summary>
    public class VolumeService
    {
        private VolumeData _volumeData;
        private AudioSource _audioSource;

        [Inject]
        public void Construct(
            VolumeData volumeData,
            AudioSource audioSource
        )
        {
            _volumeData = volumeData;
            _audioSource = audioSource;
        }

        public float GetVolume()
        {
            return _volumeData.GetVolume();
        }

        public void SetVolume(float volume)
        {
            _volumeData.SetVolume(volume);
            _audioSource.volume = volume;
        }
    }
}
