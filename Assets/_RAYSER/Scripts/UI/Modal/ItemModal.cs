using System;
using System.Threading;
using _RAYSER.Scripts.Item;
using _RAYSER.Scripts.UI.Dialog;
using Cysharp.Threading.Tasks;
using Event.Signal;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _RAYSER.Scripts.UI.Modal
{
    /// <summary>
    /// ItemModal固有の表示・非表示処理
    /// </summary>
    public class ItemModal : MonoBehaviour, IModal
    {
        public event Action<bool> OnModalStateChanged;

        UIActiveSetter _uiActiveSetter = new UIActiveSetter();
        private IWindowUI _iuiImplementation;

        UIEffect _uiEffect = new UIEffect();
        private IUIEffect _ieffectImplementation;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private IDisposable gamePadCancelSubscription;

        [SerializeField] private ItemList itemList;
        [SerializeField] private ItemBuyButton itemButtonPrefab;
        [SerializeField] private Transform contentTransform;
        [SerializeField] private CanvasGroup contentCanvasGroup;
        [SerializeField] private Button _closeButton;
        [SerializeField] private ItemDialog itemDialog;

        /// <summary>
        /// 最初にフォーカスになるUIのボタン
        /// </summary>
        [SerializeField] private GameObject firstFocusUI;
        private void OnGamePadCancel()
        {
            if (gameObject.activeSelf) // モーダルが表示されているか確認
            {
                // モーダルを閉じる処理
                Hide().Forget();
            }
        }
        private void Awake()
        {
            MessageBroker.Default.Publish(new UISelectorSignal { forcusUIGameObject = firstFocusUI });

            _closeButton.OnClickAsObservable().Subscribe(
                _ => { OnGamePadCancel(); }
            ).AddTo(this);

            gamePadCancelSubscription = MessageBroker.Default
                .Receive<GamePadCancelButtonPush>()
                .Subscribe(_ => OnGamePadCancel())
                .AddTo(this);
        }

        private void OnDestroy()
        {
            gamePadCancelSubscription?.Dispose();
        }

        private void OnItemDialogStateChanged(bool isDialogOpen)
        {
            if (isDialogOpen)
            {
                // ItemDialogが開いている間は、Closeボタンとゲームパッドのキャンセルボタンを無効にする
                _closeButton.interactable = false;
                gamePadCancelSubscription?.Dispose();
            }
            else
            {
                // ItemDialogが閉じたら、Closeボタンとゲームパッドのキャンセルボタンを有効にする
                _closeButton.interactable = true;
                gamePadCancelSubscription = MessageBroker.Default
                    .Receive<GamePadCancelButtonPush>()
                    .Subscribe(_ => OnGamePadCancel())
                    .AddTo(this);
                MessageBroker.Default.Publish(new UISelectorSignal { forcusUIGameObject = firstFocusUI });
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstFocusUI);
            }
        }

        public void Setup(ItemDialog itemDialog)
        {
            this.itemDialog = itemDialog;
            // ItemDialogの状態変更イベントを購読
            itemDialog.OnDialogStateChanged += OnItemDialogStateChanged;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            // _uiActiveSetter.SetActive(gameObject, isActive);
        }

        public async UniTask Show()
        {
            try
            {
                OnModalStateChanged?.Invoke(true);
                SetActive(true);
                // InitializeUI();

                await _uiEffect.FadeIn(contentCanvasGroup, cts.Token);

                // ライセンスUI表示
                // await _uiEffect.FadeIn(windowCanvasGroup, cts.Token);
                // await _uiEffect.SizeDelta(windowRectTransform, new Vector2(_initialUISizeDelta.x, setDeltaMinY),
                //     setDeltaDuration, cts.Token);
                // await _uiEffect.SizeDelta(windowRectTransform,
                //     new Vector2(_initialUISizeDelta.x, _initialUISizeDelta.y), setDeltaDuration, cts.Token);

                // 見出しイメージ表示
                // await _uiEffect.FadeIn(headerImageCanvasGroup, cts.Token);

                // テキスト表示
                // await _uiEffect.FadeIn(contentCanvasGroup, cts.Token);

                // UI内ボタン表示
                // await _uiEffect.FadeIn(insideButtonsCanvasGroup, cts.Token);

                //初期選択ボタンの再指定
                MessageBroker.Default.Publish(new UISelectorSignal { forcusUIGameObject = firstFocusUI });
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstFocusUI);
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合の処理
                Debug.Log("FadeIn Canceled");
            }
        }

        public async UniTask Hide()
        {
            // ItemModal固有の非表示処理
            OnModalStateChanged?.Invoke(false);
            this.gameObject.SetActive(false);
        }

        public bool IsActive
        {
            get { return this.gameObject.activeSelf; }
        }
    }
}
