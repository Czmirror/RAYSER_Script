using _RAYSER.Scripts.Score;
using _RAYSER.Scripts.UI.Dialog;
using _RAYSER.Scripts.UI.Modal;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテムライフタイムスコープクラス
    /// </summary>
    public class ItemLifetimeScope : LifetimeScope
    {
        [SerializeField] private ItemList itemList;
        [SerializeField] private ItemBuyButton itemButtonPrefab;
        [SerializeField] private Transform itemModalContentTransform;
        [SerializeField] private ItemModal itemModal;
        [SerializeField] private ItemDialog itemDialog;
        [SerializeField] private Transform itemModalTransform;

        private ItemDialog _itemDialogInstance;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<DialogOpenSignal>(options);
            builder.RegisterMessageBroker<DialogCloseSignal>(options);

            if (itemList != null)
            {
                builder.RegisterInstance(itemList);
            }

            builder.RegisterBuildCallback(container =>
            {
                _itemDialogInstance = Instantiate(itemDialog, itemModalTransform);
                var itemPurchaseSignalPublisher = container.Resolve<IPublisher<ItemPurchaseSignal>>();
                var dialogCloseSignalPublisher = container.Resolve<IPublisher<DialogCloseSignal>>();
                var dialogOpenSignalSubscriber = container.Resolve<ISubscriber<DialogOpenSignal>>();
                var dialogCloaseSignalSubscriber = container.Resolve<ISubscriber<DialogCloseSignal>>();
                _itemDialogInstance.Setup(
                    itemPurchaseSignalPublisher,
                    dialogCloseSignalPublisher,
                    dialogOpenSignalSubscriber,
                    dialogCloaseSignalSubscriber);

                var scoreData = container.Resolve<ScoreData>();
                var itemAcquisition = container.Resolve<ItemAcquisition>();
                foreach (var item in itemList.items)
                {
                    var itemBuyButton = Instantiate(itemButtonPrefab, itemModalContentTransform);
                    var publisher = container.Resolve<IPublisher<DialogOpenSignal>>();
                    itemBuyButton.Setup(item, publisher, scoreData, _itemDialogInstance, itemAcquisition);
                }

                itemModal.Setup(_itemDialogInstance);
            });
        }
    }
}
