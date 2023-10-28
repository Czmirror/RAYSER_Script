using _RAYSER.Scripts.Item;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.Weapon
{
    /// <summary>
    /// サブウェポン用LifetimeScope
    /// </summary>
    public class SubWeaponLifetimeScope : LifetimeScope
    {
        [SerializeField] private ItemBuyButton[] itemButtons = default;
        protected override void Configure(IContainerBuilder builder)
        {
            // MessagePipeの設定
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<CurrentSubWeaponIndex>(options);

            if (itemButtons != null) {
                for (int i = 0; i < itemButtons.Length; i++) {
                    itemButtons[i].Id = i; // ID を設定
                    builder.RegisterInstance(itemButtons[i]).As<ItemBuyButton>();
                }
            }
        }
    }
}
