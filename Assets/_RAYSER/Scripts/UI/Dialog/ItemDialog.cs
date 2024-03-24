using System;
using _RAYSER.Scripts.Item;
using Event.Signal;
using MessagePipe;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _RAYSER.Scripts.UI.Dialog
{
    /// <summary>
    /// アイテム購入・キャンセルダイアログクラス
    /// </summary>
    public class ItemDialog : MonoBehaviour, IDisposable
    {
        /// <summary>
        /// 開閉状態変更イベント
        /// </summary>
        public event Action<bool> OnDialogStateChanged;

        private IPublisher<ItemPurchaseSignal> _itemPurchaseProcessingPublisher;
        private IPublisher<DialogCloseSignal> _dialogCloseSignalPublisher;
        private ISubscriber<DialogOpenSignal> _dialogOpenSignalSubscriber;
        private ISubscriber<DialogCloseSignal> _dialogCloseSignalSubscriber;
        private IDisposable _dialogOpenSignalDisposable;
        private IDisposable _dialogCloseSignalDisposable;
        private IDisposable gamePadCancelSubscription;
        private ItemData _currentItemData;

        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private Image itemImage;

        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button closeButton;

        /// <summary>
        /// 最初にフォーカスになるUIのボタン
        /// </summary>
        [SerializeField] private GameObject firstFocusUI;

        private void OnGamePadCancel()
        {
            // ダイアログが表示されている場合にのみ、ダイアログを閉じる
            if (gameObject.activeSelf)
            {
                _dialogCloseSignalPublisher.Publish(new DialogCloseSignal());
            }
        }

        public void Setup(
            IPublisher<ItemPurchaseSignal> itemPurchaseSignalPublisher,
            IPublisher<DialogCloseSignal> dialogCloseSignalPublisher,
            ISubscriber<DialogOpenSignal> openDialogsubscriber,
            ISubscriber<DialogCloseSignal> closeDialogsubscriber)
        {
            _itemPurchaseProcessingPublisher = itemPurchaseSignalPublisher;
            _dialogCloseSignalPublisher = dialogCloseSignalPublisher;
            _dialogOpenSignalSubscriber = openDialogsubscriber;
            _dialogCloseSignalSubscriber = closeDialogsubscriber;

            var d = DisposableBag.CreateBuilder();



            gameObject.SetActive(false);

            purchaseButton.onClick.AddListener(() =>
            {
                if (_currentItemData != null)
                {
                    _itemPurchaseProcessingPublisher.Publish(new ItemPurchaseSignal(_currentItemData));
                    _dialogCloseSignalPublisher.Publish(new DialogCloseSignal());
                }
            });


            closeButton.onClick.AddListener(() => { _dialogCloseSignalPublisher.Publish(new DialogCloseSignal()); });

            _dialogOpenSignalSubscriber
                .Subscribe(signal => { show(signal.Item); }).AddTo(d);
            _dialogOpenSignalDisposable = d.Build();

            _dialogCloseSignalSubscriber
                .Subscribe(signal => { hide(); })
                .AddTo(d);
            _dialogCloseSignalDisposable = d.Build();

            // ゲームパッドのキャンセルボタンの購読を開始
            gamePadCancelSubscription = MessageBroker.Default
                .Receive<GamePadCancelButtonPush>()
                .Subscribe(_ => OnGamePadCancel())
                .AddTo(this);
        }

        /// <summary>
        /// 購読解除
        /// </summary>
        public void Dispose()
        {
            _dialogOpenSignalDisposable?.Dispose();
            _dialogCloseSignalDisposable?.Dispose();
        }

        private void show(IItem item)
        {
            OnDialogStateChanged?.Invoke(true);

            _currentItemData = item as ItemData; // itemがItemData型であれば、それを_currentItemDataに割り当てる
            gameObject.SetActive(true);
            itemNameText.text = item.name;
            itemPriceText.text = item.requiredScore.ToString();
            itemImage.sprite = item.iconImage;

            MessageBroker.Default.Publish(new UISelectorSignal { forcusUIGameObject = purchaseButton.gameObject });
            MessageBroker.Default.Publish(new UISelectorSignal { forcusUIGameObject = closeButton.gameObject });
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstFocusUI);
        }

        private void hide()
        {
            OnDialogStateChanged?.Invoke(false);

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
