using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _RAYSER.Scripts.UI.Title
{
    public class ConfigWindowUI : MonoBehaviour, IWindowUI
    {
        UIActiveSetter _uiActiveSetter = new UIActiveSetter();
        private IWindowUI _iuiImplementation;

        UIEffect _uiEffect = new UIEffect();
        private IUIEffect _ieffectImplementation;

        private Vector3 _initialPosition = Vector3.zero;
        private Vector2 _initialUISizeDelta = Vector2.zero;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private float setDeltaMinx = 0;
        private float setDeltaMinY = 2f;
        private float setDeltaDuration = 1f;

        /// <summary>
        /// 左スティックの入力値
        /// </summary>
        private float leftStickInputValue;

        /// <summary>
        /// 左スティックの入力感度
        /// </summary>
        public float leftStickSensitivity = 0.01f;

        [SerializeField] private CanvasGroup windowCanvasGroup;
        [SerializeField] private RectTransform windowRectTransform;
        [SerializeField] private CanvasGroup headerImageCanvasGroup;
        [SerializeField] private CanvasGroup insideButtonsCanvasGroup;
        [SerializeField] private CanvasGroup contentCanvasGroup;

        /// <summary>
        /// 最初にフォーカスになるライセンスUIのボタン
        /// </summary>
        [SerializeField] private GameObject firstFocusUI;

        [SerializeField] private PlayerInputNavigate playerInputNavigate;
        [SerializeField] private Slider slider;

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

            SetActive(false);
        }

        private void Update()
        {
            var _moveDirection = playerInputNavigate.GetDirection();
            var horizon = _moveDirection.x;

            if (horizon != 0)
            {
                slider.value += horizon * leftStickSensitivity;
            }
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

            windowRectTransform.position = UIPositionCalculator.CalculateCenterPosition(windowRectTransform);
        }

        public async UniTask ShowUI()
        {
            try
            {
                SetActive(true);
                InitializeUI();

                // UI表示
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

                //初期選択ボタンの再指定
                EventSystem.current.SetSelectedGameObject(firstFocusUI);
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

                // ライセンスUI非表示
                await _uiEffect.FadeOut(windowCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(windowRectTransform, new Vector2(_initialUISizeDelta.x, setDeltaMinY),
                    setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(windowRectTransform,
                    new Vector2(setDeltaMinx, setDeltaMinY), setDeltaDuration, cts.Token);

                // UI無効
                SetActive(false);
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合の処理
                Debug.Log("Fade Canceled");
            }
        }
    }
}
