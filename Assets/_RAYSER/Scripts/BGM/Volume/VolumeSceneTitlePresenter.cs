using BGM.Volume;
using VContainer.Unity;

namespace _RAYSER.Scripts.BGM.Volume
{
    /// <summary>
    /// 音量調整プレゼンター
    /// </summary>
    public class VolumeSceneTitlePresenter : IStartable
    {
        readonly VolumeService volumeService;
        readonly VolumeSlider volumeSlider;

        public VolumeSceneTitlePresenter(VolumeService volumeService, VolumeSlider volumeSlider)
        {
            this.volumeService = volumeService;
            this.volumeSlider = volumeSlider;
        }

        void IStartable.Start()
        {
            // スライダーの値をVolumeServiceに渡す
            volumeSlider.slider.onValueChanged.AddListener(volumeService.SetVolume);
        }
    }
}
