using BGM.Volume;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.BGM.Volume
{
    /// <summary>
    /// 音量受信 GameSceneのGameObjectにアタッチ
    /// </summary>
    public sealed class VolumeSceneGameLifetimeScope : LifetimeScope
    {
        [SerializeField] private AudioSource audioSource;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<VolumeService>(Lifetime.Singleton);
            builder.Register<VolumeSceneGameSetter>(Lifetime.Singleton);
            builder.RegisterEntryPoint<VolumeSceneGameSetter>();
            builder.RegisterInstance(audioSource);
        }
    }
}
