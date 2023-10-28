using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _RAYSER.Scripts.UI.Title
{
    /// <summary>
    /// ライセンスUIクラス
    /// </summary>
    public class LicenseWindowUI : MonoBehaviour, IWindowUI
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

        [SerializeField] private CanvasGroup licenseCanvasGroup;
        [SerializeField] private RectTransform licenseRectTransform;
        [SerializeField] private CanvasGroup headerImageCanvasGroup;
        [SerializeField] private CanvasGroup insideButtonsCanvasGroup;
        [SerializeField] private CanvasGroup contentCanvasGroup;

        /// <summary>
        /// 最初にフォーカスになるライセンスUIのボタン
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
            _initialPosition = licenseRectTransform.position;
            _initialUISizeDelta = licenseRectTransform.sizeDelta;

            InitializeUI();

            // UI無効
            SetActive(false);
        }

        public void SetActive(bool isActive)
        {
            _uiActiveSetter.SetActive(gameObject, isActive);
        }

        private void InitializeUI()
        {
            _uiEffect.SetAlphaZero(licenseCanvasGroup);
            _uiEffect.SetSizeDeltaZero(licenseRectTransform);
            _uiEffect.SetAlphaZero(headerImageCanvasGroup);
            _uiEffect.SetAlphaZero(insideButtonsCanvasGroup);
            _uiEffect.SetAlphaZero(contentCanvasGroup);
        }

        public async UniTask ShowUI()
        {
            try
            {
                SetActive(true);
                InitializeUI();

                // ライセンスUI表示
                await _uiEffect.FadeIn(licenseCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(licenseRectTransform, new Vector2(_initialUISizeDelta.x, setDeltaMinY),
                    setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(licenseRectTransform,
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
                await _uiEffect.FadeOut(licenseCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(licenseRectTransform, new Vector2(_initialUISizeDelta.x, setDeltaMinY),
                    setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(licenseRectTransform,
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
