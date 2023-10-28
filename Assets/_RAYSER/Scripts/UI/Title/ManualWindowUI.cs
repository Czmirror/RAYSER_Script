using System;
using System.Threading;
using _RAYSER.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Title
{
    /// <summary>
    /// マニュアルUIクラス
    /// </summary>
    public class ManualWindowUI : MonoBehaviour, IWindowUI
    {
        UIActiveSetter _uiActiveSette = new UIActiveSetter();
        private IWindowUI _iuiImplementation;

        UIEffect _uiEffect = new UIEffect();
        private IUIEffect _ieffectImplementation;

        private Vector3 _initialPosition = Vector3.zero;
        private Vector2 _initialUISizeDelta = Vector2.zero;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private float setDeltaMinx = 0;
        private float setDeltaMinY = 2f;
        private float setDeltaDuration = 1f;
        private float hiddeenInsideFadeOutValue = 0;
        private float hiddeenInsideFadeDuration = 0.1f;

        [SerializeField] private CanvasGroup manualCanvasGroup;
        [SerializeField] private RectTransform manualRectTransform;
        [SerializeField] private CanvasGroup manualInsideButtonsCanvasGroup;
        [SerializeField] private CanvasGroup manualHeaderImageCanvasGroup;
        [SerializeField] private CanvasGroup controlsCanvasGroup;
        [SerializeField] private RectTransform controlsRectTransform;
        [SerializeField] private CanvasGroup storyCanvasGroup;
        [SerializeField] private RectTransform storyRectTransform;
        [SerializeField] private CanvasGroup charactersCanvasGroup;
        [SerializeField] private RectTransform charactersRectTransform;
        [SerializeField] private CanvasGroup gameScreenCanvasGroup;
        [SerializeField] private RectTransform gameScreenRectTransform;

        /// <summary>
        /// 最初にフォーカスになるライセンスUIのボタン
        /// </summary>
        [SerializeField] private GameObject firstFocusUI;

        public void SetActive(bool isActive)
        {
            _uiActiveSette.SetActive(gameObject, isActive);
        }

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

        public void InitializeUI()
        {
            manualCanvasGroup.alpha = 0;
            manualRectTransform.sizeDelta = Vector2.zero;
            manualInsideButtonsCanvasGroup.alpha = 0;
            manualHeaderImageCanvasGroup.alpha = 0;
            controlsCanvasGroup.alpha = 0;
            storyCanvasGroup.alpha = 0;
            charactersCanvasGroup.alpha = 0;
            gameScreenCanvasGroup.alpha = 0;
        }

        private void Awake()
        {
            _initialPosition = manualRectTransform.position;
            _initialUISizeDelta = manualRectTransform.sizeDelta;

            InitializeUI();

            // UI無効
            SetActive(false);
        }

        public async UniTask ShowUI()
        {
            try
            {
                SetActive(true);
                InitializeUI();

                manualRectTransform.position = _initialPosition;

                // マニュアル表示
                await _uiEffect.FadeIn(manualCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(manualRectTransform, new Vector2(_initialUISizeDelta.x, setDeltaMinY),
                    setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(manualRectTransform,
                    new Vector2(_initialUISizeDelta.x, _initialUISizeDelta.y), setDeltaDuration, cts.Token);

                // マニュアル見出しイメージ表示
                await _uiEffect.FadeIn(manualHeaderImageCanvasGroup, cts.Token);

                // マニュアル内ボタン表示
                await _uiEffect.FadeIn(manualInsideButtonsCanvasGroup, cts.Token);

                await ShowControls();

                //初期選択ボタンの再指定
                EventSystem.current.SetSelectedGameObject(firstFocusUI);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

        }

        public async UniTask HideUI()
        {
            try
            {
                //初期選択ボタンの初期化
                EventSystem.current.SetSelectedGameObject(null);

                // 見出しイメージ非表示
                await _uiEffect.FadeOut(manualHeaderImageCanvasGroup, cts.Token);

                await HiddenInsideUI();

                // UI内ボタン非表示
                await _uiEffect.FadeOut(manualInsideButtonsCanvasGroup, cts.Token);

                // マニュアル非表示
                await _uiEffect.FadeOut(manualCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(manualRectTransform, new Vector2(_initialUISizeDelta.x, setDeltaMinY),
                    setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(manualRectTransform,
                    new Vector2(setDeltaMinx, setDeltaMinY), setDeltaDuration, cts.Token);

                // UI無効
                SetActive(false);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

        }

        /// <summary>
        /// 操作説明、ストーリー、キャラクター非表示
        /// </summary>
        private async UniTask HiddenInsideUI()
        {
            await _uiEffect.Fade(controlsCanvasGroup,hiddeenInsideFadeOutValue,hiddeenInsideFadeDuration, cts.Token);
            await _uiEffect.Fade(storyCanvasGroup,hiddeenInsideFadeOutValue,hiddeenInsideFadeDuration, cts.Token);
            await _uiEffect.Fade(charactersCanvasGroup,hiddeenInsideFadeOutValue,hiddeenInsideFadeDuration, cts.Token);
            await _uiEffect.Fade(gameScreenCanvasGroup,hiddeenInsideFadeOutValue,hiddeenInsideFadeDuration, cts.Token);
        }

        private async UniTask ShowInsideUI(CanvasGroup canvasGroup, float fadeDuration)
        {
            await _uiEffect.FadeIn(canvasGroup, cts.Token);
        }

        public async UniTask ShowControls()
        {
            await HiddenInsideUI();
            await ShowInsideUI(controlsCanvasGroup, 1f);
        }

        public async UniTask ShowStory()
        {
            await HiddenInsideUI();
            await ShowInsideUI(storyCanvasGroup, 1f);
        }

        public async UniTask ShowCharacters()
        {
            await HiddenInsideUI();
            await ShowInsideUI(charactersCanvasGroup, 1f);
        }

        public async UniTask ShowGameScreen()
        {
            await HiddenInsideUI();
            await ShowInsideUI(gameScreenCanvasGroup, 1f);
        }
    }
}
