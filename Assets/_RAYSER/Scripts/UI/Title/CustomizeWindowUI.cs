using System;
using System.Collections;
using System.Threading;
using _RAYSER.Scripts.UI.Modal;
using Cysharp.Threading.Tasks;
using Event.Signal;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _RAYSER.Scripts.UI.Title
{
    /// <summary>
    /// カスタマイズウィンドウUI
    /// </summary>
    public class CustomizeWindowUI : MonoBehaviour, IWindowUI, IModalable
    {
        UIActiveSetter _uiActiveSetter = new UIActiveSetter();
        private IWindowUI _iuiImplementation;

        UIEffect _uiEffect = new UIEffect();
        private IUIEffect _ieffectImplementation;

        private Vector3 _initialPosition = Vector3.zero;
        private Vector2 _initialUISizeDelta = Vector2.zero;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private IDisposable gamePadCancelSubscription;

        private float setDeltaMinx = 0;
        private float setDeltaMinY = 2f;
        private float setDeltaDuration = 1f;

        [SerializeField] private CanvasGroup windowCanvasGroup;
        [SerializeField] private RectTransform windowRectTransform;
        [SerializeField] private CanvasGroup headerImageCanvasGroup;
        [SerializeField] private CanvasGroup insideButtonsCanvasGroup;
        [SerializeField] private CanvasGroup contentCanvasGroup;
        [SerializeField] private CanvasGroup backImageCanvasGroup;
        [SerializeField] private Button[] buttons;
        [SerializeField] private EquipmentModal equipmentModal;
        [SerializeField] private ItemModal itemModal;

        /// <summary>
        /// メニューUI
        /// </summary>
        [SerializeField] private TitleMenuButtonsUI titleMenuButtonsUI;

        [SerializeField] private Button _equipmentButton;

        /// <summary>
        /// アイテムボタン
        /// </summary>
        [SerializeField] private Button _itemButton;

        /// <summary>
        /// 閉じるボタン
        /// </summary>
        [SerializeField] private Button _closeButton;

        /// <summary>
        /// 最初にフォーカスになるUIのボタン
        /// </summary>
        [SerializeField] private GameObject firstFocusUI;

        public UIActiveSetter UIActiveSetter
        {
            get => _iuiImplementation.UIActiveSetter;
            set => _iuiImplementation.UIActiveSetter = value;
        }

        /// <summary>
        /// キャンセルトークンにキャンセル要求を発行する
        /// </summary>
        private void Cancel()
        {
            cts.Cancel();
        }

        private void Awake()
        {
            _initialPosition = windowRectTransform.position;
            _initialUISizeDelta = windowRectTransform.sizeDelta;

            InitializeUI();

            foreach (var button in buttons)
            {
                MessageBroker.Default.Publish(new UISelectorSignal { forcusUIGameObject = button.gameObject });
            }

            // UI無効
            // SetActive(false);

            _equipmentButton.OnClickAsObservable().Subscribe(
                _ => { PushButtonEquipment(); }
            ).AddTo(this);

            _itemButton.OnClickAsObservable().Subscribe(
                _ => { PushButtonItem(); }
            ).AddTo(this);

            _closeButton.OnClickAsObservable().Subscribe(
                _ => { HideUI(); }
            ).AddTo(this);

            gamePadCancelSubscription = MessageBroker.Default
                .Receive<GamePadCancelButtonPush>()
                .Where(_ => gameObject.activeSelf)
                .Subscribe(_ => OnGamePadCancel())
                .AddTo(this);

            itemModal.OnModalStateChanged += SetButtonsInteractable;

            // UI無効
            SetActive(false);
        }

        private void OnGamePadCancel()
        {
            // ItemModalが表示されている場合は何もしない
            if (itemModal.IsActive)
            {
                return;
            }

            // キャンセルボタンが押された時の処理
            HideUI().Forget();
        }

        private void OnDestroy()
        {
            gamePadCancelSubscription?.Dispose();
            itemModal.OnModalStateChanged -= SetButtonsInteractable;
        }

        public void SetActive(bool isActive)
        {
            _uiActiveSetter.SetActive(gameObject, isActive);
        }

        private void InitializeUI()
        {
            _uiEffect.SetAlphaZero(windowCanvasGroup);
            _uiEffect.SetSizeDeltaZero(windowRectTransform);
            _uiEffect.SetAlphaZero(headerImageCanvasGroup);
            _uiEffect.SetAlphaZero(insideButtonsCanvasGroup);
            _uiEffect.SetAlphaZero(contentCanvasGroup);
            _uiEffect.SetAlphaZero(backImageCanvasGroup);
            itemModal.Hide();
        }

        /// <summary>
        /// ボタンの有効・無効化切り替え処理
        /// </summary>
        /// <param name="interactable"></param>
        private void SetButtonsInteractable(bool interactable)
        {
            foreach (var button in buttons)
            {
                // モーダル非表示の場合、CustomizeUIのボタンを有効化、モーダル表示の場合、CustomizeUIのボタンを無効化
                button.interactable = !interactable;
            }

            if (!interactable && gameObject.activeSelf)
            {
                MessageBroker.Default.Publish(new UISelectorSignal { forcusUIGameObject = firstFocusUI });
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstFocusUI);
            }
        }

        public async UniTask ShowUI()
        {
            try
            {
                MessageBroker.Default.Publish(new ScoreAccumulation { Score = 1000000 });
                SetActive(true);
                InitializeUI();

                // ライセンスUI表示
                await _uiEffect.FadeIn(windowCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(windowRectTransform, new Vector2(_initialUISizeDelta.x, setDeltaMinY),
                    setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(windowRectTransform,
                    new Vector2(_initialUISizeDelta.x, _initialUISizeDelta.y), setDeltaDuration, cts.Token);

                // 見出しイメージ表示
                await _uiEffect.FadeIn(headerImageCanvasGroup, cts.Token);

                // テキスト表示
                await _uiEffect.FadeIn(contentCanvasGroup, cts.Token);

                // UI内ボタン表示
                await _uiEffect.FadeIn(insideButtonsCanvasGroup, cts.Token);

                await _uiEffect.FadeIn(backImageCanvasGroup, cts.Token);

                // UI内ボタン有効化
                SetButtonsInteractable(itemModal.IsActive);
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合の処理
                Debug.Log("FadeIn Canceled");
            }
        }

        public async UniTask HideUI()
        {
            try
            {
                //初期選択ボタンの初期化
                EventSystem.current.SetSelectedGameObject(null);

                // 見出しイメージ非表示
                await _uiEffect.FadeOut(headerImageCanvasGroup, cts.Token);

                // テキスト非表示
                await _uiEffect.FadeOut(contentCanvasGroup, cts.Token);

                // UI内ボタン非表示
                await _uiEffect.FadeOut(insideButtonsCanvasGroup, cts.Token);
                await _uiEffect.FadeOut(backImageCanvasGroup, cts.Token);

                // UI非表示
                await _uiEffect.FadeOut(windowCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(windowRectTransform, new Vector2(_initialUISizeDelta.x, setDeltaMinY),
                    setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(windowRectTransform,
                    new Vector2(setDeltaMinx, setDeltaMinY), setDeltaDuration, cts.Token);

                // UI無効
                SetActive(false);

                await titleMenuButtonsUI.ShowUI();
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合の処理
                Debug.Log("Fade Canceled");
            }
        }

        public async UniTask ToggleModal(IModal modal)
        {
            bool isShowing = modal.IsActive;
            Debug.Log("ToggleModal " + isShowing);
            SetButtonsInteractable(!isShowing);
            if (isShowing)
            {
                modal.Hide();
            }
            else
            {
                await modal.Show();
            }
        }

        public void ShowEquipmentModal()
        {
            ToggleModal(equipmentModal);
        }

        public void ShowItemModal()
        {
            ToggleModal(itemModal);
        }

        public async UniTask PushButtonEquipment()
        {
            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);

            await equipmentModal.Show();
        }

        public async UniTask PushButtonItem()
        {
            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);

            await itemModal.Show();
        }
    }
}
