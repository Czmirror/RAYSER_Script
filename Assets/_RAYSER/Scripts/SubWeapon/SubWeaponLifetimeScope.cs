using _RAYSER.Scripts.Bomb;
using _RAYSER.Scripts.Item;
using InputSystem;
using MessagePipe;
using Shield;
using UI.Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// サブウェポン用LifetimeScope
    /// </summary>
    public class SubWeaponLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerController playerController;

        // サブウェポン
        [SerializeField] private SubWeaponTurret subWeaponTurret;
        [SerializeField] private SubWeaponsUI subWeaponsUI;
        [SerializeField] private Transform subWeaponIndividualParent;
        [SerializeField] private SubWeaponIndividual subWeaponIndividualPrefab;
        [SerializeField] private ItemData vulcan;
        [SerializeField] private ItemData threeWay;
        [SerializeField] private ItemData homing;
        [SerializeField] private ItemData spiral;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PlayerInputSetupService>(Lifetime.Singleton);

            builder.RegisterBuildCallback(container =>
            {
                var setupService = container.Resolve<PlayerInputSetupService>();
                setupService.SetupPlayerController(playerController, container);

                var itemAcquisition = container.Resolve<ItemAcquisition>();
                itemAcquisition.debugAddItem(vulcan);
                itemAcquisition.debugAddItem(threeWay);
                itemAcquisition.debugAddItem(homing);
                itemAcquisition.debugAddItem(spiral);

                var subweaponUseSignalSubscriber = container.Resolve<ISubscriber<SubweaponUseSignal>>();
                subWeaponTurret.Setup(subweaponUseSignalSubscriber, itemAcquisition);

                subWeaponsUI.Setup(itemAcquisition);
                var subweaponMoveDirectionSubscriber = container.Resolve<ISubscriber<SubweaponMoveDirection>>();

                foreach (var item in itemAcquisition.GetSubWeapon())
                {
                    if (item is ItemData data)
                    {
                        var subWeaponIndividual = Instantiate(subWeaponIndividualPrefab, subWeaponIndividualParent);
                        subWeaponIndividual.Setup(itemAcquisition, subweaponMoveDirectionSubscriber, data);
                    }
                }
            });
        }
    }
}
