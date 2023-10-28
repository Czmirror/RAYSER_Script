using _RAYSER.Scripts.Item;
using _RAYSER.Scripts.Score;
using _RAYSER.Scripts.Weapon;
using BGM.Volume;
using Score;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.VContainer
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            // 子のLifetimeScopeに同じVolumeDataを引き渡す
            builder.Register<VolumeData>(Lifetime.Singleton);
            builder.Register<ScoreData>(Lifetime.Singleton);
            builder.Register<ItemAcquisition>(Lifetime.Singleton);
            builder.Register<SubWeaponMounted>(Lifetime.Singleton);
        }
    }
}
