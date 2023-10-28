using BGM.Volume;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.BGM.Volume
{
    /// <summary>
    /// ゲームシーン 音量初期設定
    /// </summary>
    public class VolumeSceneGameSetter : IStartable
    {
        private AudioSource _audioSource;
        private VolumeData _volumeData;
        readonly VolumeService _volumeService;

        [Inject]
        public void Construct(VolumeData volumeData)
        {
            _volumeData = volumeData;
        }

        public VolumeSceneGameSetter(
            AudioSource audioSource,
            VolumeData volumeData,
            VolumeService volumeService
            )
        {
            _audioSource = audioSource;
            _volumeData = volumeData;
            _volumeService = volumeService;
        }

        void IStartable.Start()
        {
            this._audioSource.volume = _volumeData.GetVolume();
        }
    }
}
