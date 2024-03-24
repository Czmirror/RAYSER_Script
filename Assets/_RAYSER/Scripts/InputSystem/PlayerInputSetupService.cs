using _RAYSER.Scripts.Bomb;
using _RAYSER.Scripts.Item;
using _RAYSER.Scripts.SubWeapon;
using MessagePipe;
using VContainer;

namespace InputSystem
{
    public class PlayerInputSetupService
    {
        public void SetupPlayerController(PlayerController playerController, IObjectResolver resolver)
        {
            var subweaponMoveDirectionPublisher = resolver.Resolve<IPublisher<SubweaponMoveDirection>>();
            var subweaponUseSignalPublisher = resolver.Resolve<IPublisher<SubweaponUseSignal>>();
            var bombUseSignalPublisher = resolver.Resolve<IPublisher<BombUseSignal>>();
            var itemAcquisition = resolver.Resolve<ItemAcquisition>();

            playerController.Setup(subweaponMoveDirectionPublisher, subweaponUseSignalPublisher, bombUseSignalPublisher, itemAcquisition);
        }
    }
}
