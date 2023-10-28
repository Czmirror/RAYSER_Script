using _RAYSER.Scripts.BGM.Volume;
using _RAYSER.Scripts.VContainer;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BGM.Volume
{
    /// <summary>
    /// タイトルの音量受信のライフタイムスコープ
    /// タイトルシーンのGameObjectにアタッチして使用する
    /// </summary>
    public class VolumeSceneTitleLifetimeScope : LifetimeScope
    {
        [SerializeField] private VolumeSlider _volumeSlider;
        [SerializeField] private AudioSource audioSource;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<VolumeService>(Lifetime.Singleton);
            builder.Register<VolumeSceneTitlePresenter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<VolumeSceneTitlePresenter>();
            builder.RegisterComponent(_volumeSlider);
            builder.RegisterInstance(audioSource);
        }
    }
}
