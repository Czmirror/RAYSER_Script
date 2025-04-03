using _RAYSER.Scripts.Bomb;
using _RAYSER.Scripts.Item;
using _RAYSER.Scripts.Score;
using _RAYSER.Scripts.SubWeapon;
using BGM.Volume;
using MessagePipe;
using Status;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.VContainer
{
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            // 子のLifetimeScopeに同じVolumeDataとScoreDataを引き渡す
            builder.Register<VolumeData>(Lifetime.Singleton);
            builder.Register<ScoreData>(Lifetime.Singleton);
            builder.Register<CurrentGameState>(Lifetime.Singleton);

            // MessagePipeの設定
            var messagePipeOptions = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<ItemPurchaseSignal>(messagePipeOptions);
            builder.RegisterMessageBroker<SubweaponMoveDirection>(messagePipeOptions);
            builder.RegisterMessageBroker<ItemData>(messagePipeOptions);
            builder.RegisterMessageBroker<SubweaponUseSignal>(messagePipeOptions);
            builder.RegisterMessageBroker<BombUseSignal>(messagePipeOptions);
            builder.RegisterMessageBroker<BombActiveSignal>(messagePipeOptions);

            // ItemAcquisitionをシングルトンとして登録
            builder.Register<ItemAcquisition>(Lifetime.Singleton);

            // SubWeaponMountedをシングルトンとして登録
            builder.Register<SubWeaponMounted>(Lifetime.Singleton);
        }
    }
}
