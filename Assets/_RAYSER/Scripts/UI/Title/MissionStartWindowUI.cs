using System;
using System.Threading;
using _RAYSER.Scripts.UI.Title;
using Cysharp.Threading.Tasks;
using Event.Signal;
using Rayser.CustomEditor;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRM;

namespace _RAYSER.Scripts.UI
{
    public class MissionStartWindowUI : MonoBehaviour, IWindowUI
    {
        UIActiveSetter _uiActiveSetter = new UIActiveSetter();
        private IWindowUI _iuiImplementation;

        UIEffect _uiEffect = new UIEffect();
        private IUIEffect _ieffectImplementation;

        private Vector3 _initialGameStartUIPosition = Vector3.zero;
        private Vector2 _initialGameStartUISizeDelta = Vector2.zero;
        private Vector3 _initialMissionWindowPosition = Vector3.zero;
        private Vector2 _initialMissionWindowSizeDelta = Vector2.zero;

        /// <summary>
        /// 会話シーンテキスト
        /// </summary>
        private String _messege;

        /// <summary>
        /// 会話シーン文字送り速度
        /// </summary>
        private float _message_speed = 0.05f;

        private CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// setDeltaX最小値
        /// </summary>
        private float setDeltaMinx = 0;

        /// <summary>
        /// setDeltaY最小値
        /// </summary>
        private float setDeltaMinY = 2f;

        /// <summary>
        /// setDeltaDuration処理速度
        /// </summary>
        private float setDeltaDuration = 1f;

        /// <summary>
        /// ミッション開始光演出の最終値（上部）
        /// </summary>
        private float setmissionLightUITopEndValue = 1000;

        /// <summary>
        /// ミッション開始光演出の最終値（株）
        /// </summary>
        private float setmissionLightUIBottomEndValue = -1000;

        /// <summary>
        /// ミッション開始光演出の処理速度
        /// </summary>
        private float setmissionLightDuration = -0.5f;

        /// <summary>
        /// ゲームパッドのキャンセルボタン受付
        /// </summary>
        private bool _gamePadCancelButtonAcceptance = false;

        [SerializeField] private CanvasGroup gameStartUICanvasGroup;
        [SerializeField] private RectTransform gameStartUIRectTransform;
        [SerializeField] private CanvasGroup missionWindowCanvasGroup;
        [SerializeField] private RectTransform missionWindowRectTransform;
        [SerializeField] private RectTransform missionLightUITopRectTransform;
        [SerializeField] private RectTransform missionLightUIBottomRectTransform;
        [SerializeField] private CanvasGroup roydFaceWindowCanvasGroup;
        [SerializeField] private CanvasGroup sophieFaceWindowCanvasGroup;
        [SerializeField] private CanvasGroup roydTalkWindowCanvasGroup;
        [SerializeField] private TextMeshProUGUI roydTextMeshPro;
        [SerializeField] private CanvasGroup sophieTalkWindowCanvasGroup;
        [SerializeField] private TextMeshProUGUI sophieTextMeshPro;
        [SerializeField] private GameObject roydCamera;
        [SerializeField] private GameObject sophieCamera;
        [SerializeField] private MissionStartTalk _missionStartTalk;
        [SerializeField] private SceneObject gameScene;

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
            _initialGameStartUIPosition = gameStartUIRectTransform.position;
            _initialGameStartUISizeDelta = gameStartUIRectTransform.sizeDelta;
            _initialMissionWindowPosition = missionWindowRectTransform.position;
            _initialMissionWindowSizeDelta = missionWindowRectTransform.sizeDelta;
            InitializeUI();

            // UI無効
            SetActive(false);
        }

        private void Start()
        {
            // ゲームパッドキャンセルボタン受付
            MessageBroker.Default.Receive<GamePadCancelButtonPush>()
                .Where(_ => _gamePadCancelButtonAcceptance)
                .Subscribe(_ => Cancel())
                .AddTo(this);
        }

        public void SetActive(bool isActive)
        {
            _uiActiveSetter.SetActive(gameObject, isActive);
        }

        private void InitializeUI()
        {
            _uiEffect.SetAlphaZero(gameStartUICanvasGroup);
            _uiEffect.SetSizeDeltaZero(gameStartUIRectTransform);
            _uiEffect.SetAlphaZero(missionWindowCanvasGroup);
            _uiEffect.SetSizeDeltaZero(missionWindowRectTransform);
            _uiEffect.SetAlphaZero(roydTalkWindowCanvasGroup);
            _uiEffect.SetAlphaZero(sophieTalkWindowCanvasGroup);
            _uiEffect.SetAlphaZero(roydFaceWindowCanvasGroup);
            _uiEffect.SetAlphaZero(sophieFaceWindowCanvasGroup);
        }

        public async UniTask ShowUI()
        {
            try
            {
                SetActive(true);

                // ゲームパッドのキャンセルボタン受付
                _gamePadCancelButtonAcceptance = true;

                // GameStartUI表示
                gameStartUIRectTransform.position = _initialGameStartUIPosition;
                await _uiEffect.FadeIn(gameStartUICanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(gameStartUIRectTransform,
                    new Vector2(_initialGameStartUISizeDelta.x, setDeltaMinY), setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(gameStartUIRectTransform,
                    new Vector2(_initialGameStartUISizeDelta.x, _initialGameStartUISizeDelta.y), setDeltaDuration,
                    cts.Token);
                await _uiEffect.FadeIn(gameStartUICanvasGroup, cts.Token);

                // Mission表示
                missionWindowRectTransform.position = _initialMissionWindowPosition;
                await _uiEffect.FadeIn(missionWindowCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(missionWindowRectTransform,
                    new Vector2(_initialMissionWindowSizeDelta.x, setDeltaMinY), setDeltaDuration, cts.Token);
                await _uiEffect.SizeDelta(missionWindowRectTransform,
                    new Vector2(_initialMissionWindowSizeDelta.x, _initialMissionWindowSizeDelta.y), setDeltaDuration,
                    cts.Token);

                // Mission光演出
                _uiEffect.LocalMoveX(missionLightUITopRectTransform, setmissionLightUITopEndValue,
                    setmissionLightDuration, cts.Token);
                await _uiEffect.LocalMoveX(missionLightUIBottomRectTransform, setmissionLightUIBottomEndValue,
                    setmissionLightDuration, cts.Token);

                // ロイドカメラ有効
                roydCamera.SetActive(true);

                // ロイド顔ウインドウ表示
                _uiEffect.FadeIn(roydFaceWindowCanvasGroup, cts.Token);

                // ロイドトークウインドウ表示
                await _uiEffect.FadeIn(roydTalkWindowCanvasGroup, cts.Token);

                await _missionStartTalk.Talk(roydTextMeshPro, Character.Character.Royd, "こちらロイド、宇宙海賊の戦艦付近に到着した。",
                    _message_speed, cts.Token);

                // ソフィーカメラ有効
                sophieCamera.SetActive(true);

                // ソフィー顔ウインドウ表示
                _uiEffect.FadeIn(sophieFaceWindowCanvasGroup, cts.Token);

                // ソフィートークウインドウ表示
                await _uiEffect.FadeIn(sophieTalkWindowCanvasGroup, cts.Token);

                await _missionStartTalk.Talk(sophieTextMeshPro, Character.Character.Sophie,
                    "了解、ロイド。まずは敵戦艦付近の偵察機と思わしき数体の機体の掃討をお願い。",
                    _message_speed, cts.Token);

                await _missionStartTalk.Talk(roydTextMeshPro, Character.Character.Royd, "了解、これより攻撃を開始する",
                    _message_speed, cts.Token);
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合の処理
                Debug.Log("ShowUI Canceled");
            }
        }

        public async UniTask HideUI()
        {
            try
            {
                _uiEffect.FadeOut(roydTalkWindowCanvasGroup, cts.Token);
                await _uiEffect.FadeOut(sophieTalkWindowCanvasGroup, cts.Token);
                _uiEffect.FadeOut(roydFaceWindowCanvasGroup, cts.Token);
                await _uiEffect.FadeOut(sophieFaceWindowCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(missionWindowRectTransform,
                    new Vector2(_initialMissionWindowSizeDelta.x, setDeltaMinY),
                    setDeltaDuration, cts.Token);
                _uiEffect.FadeOut(missionWindowCanvasGroup, cts.Token);
                await _uiEffect.SizeDelta(missionWindowRectTransform, new Vector2(setDeltaMinx, setDeltaMinY),
                    setDeltaDuration, cts.Token);
            }
            catch (OperationCanceledException)
            {
                // キャンセルされた場合の処理
                Debug.Log("HideUI Canceled");
            }
            finally
            {
                // ゲームパッドのキャンセルボタン受付解除
                _gamePadCancelButtonAcceptance = false;

                SceneManager.LoadScene(gameScene);
            }
        }
    }
}
