using Cysharp.Threading.Tasks;
using Event.Signal;
using UI.Title;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _RAYSER.Scripts.UI.Title
{
    [RequireComponent(typeof(Button))]
    public class ButtonManual : MonoBehaviour
    {
        /// <summary>
        /// 対象ボタン
        /// </summary>
        [SerializeField] private Button _button;

        /// <summary>
        /// 閉じるボタン
        /// </summary>
        [SerializeField] private Button _closeButton;

        /// <summary>
        /// コントロールボタン
        /// </summary>
        [SerializeField] private Button _controlsButton;

        /// <summary>
        /// ストーリーボタン
        /// </summary>
        [SerializeField] private Button _storyButton;

        /// <summary>
        /// キャラクターボタン
        /// </summary>
        [SerializeField] private Button _charactersButton;

        /// <summary>
        /// ゲーム画面ボタン
        /// </summary>
        [SerializeField] private Button _gameScreenButton;


        /// <summary>
        /// メニューUI
        /// </summary>
        [SerializeField] private TitleMenuButtonsUI titleMenuButtonsUI;

        /// <summary>
        /// マニュアルUI
        /// </summary>
        [SerializeField] private ManualWindowUI manualWindowUI;

        /// <summary>
        /// ゲームパッドのキャンセルボタン受付
        /// </summary>
        private bool _gamePadCancelButtonAcceptance = false;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Awake()
        {
            _button.OnClickAsObservable().Subscribe(
                _ => { PushButton(); }
            ).AddTo(this);
            _closeButton.OnClickAsObservable().Subscribe(
                _ => { PushCloseButton(); }
            ).AddTo(this);
            _controlsButton.OnClickAsObservable().Subscribe(
                _ => { PushControlsButton(); }
            ).AddTo(this);
            _storyButton.OnClickAsObservable().Subscribe(
                _ => { PushStoryButton(); }
            ).AddTo(this);
            _charactersButton.OnClickAsObservable().Subscribe(
                _ => { PushCharactersButton(); }
            ).AddTo(this);
            _gameScreenButton.OnClickAsObservable().Subscribe(
                _ => { PushGameScreenButton(); }
            ).AddTo(this);
        }

        private void Start()
        {
            // ゲームパッドキャンセルボタン受付
            MessageBroker.Default.Receive<GamePadCancelButtonPush>()
                .Where(_ => _gamePadCancelButtonAcceptance)
                .Subscribe(_ => PushCloseButton())
                .AddTo(this);
        }

        public async UniTask PushButton()
        {
            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);

            await titleMenuButtonsUI.HideUI();
            await manualWindowUI.ShowUI();

            // ゲームパッドのキャンセルボタン受付
            _gamePadCancelButtonAcceptance = true;
        }

        public async UniTask PushCloseButton()
        {
            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);

            // ゲームパッドのキャンセルボタン受付解除
            _gamePadCancelButtonAcceptance = false;

            await manualWindowUI.HideUI();
            await titleMenuButtonsUI.ShowUI();
        }

        private async UniTask PushControlsButton()
        {
            await manualWindowUI.ShowControls();
        }

        private async UniTask PushStoryButton()
        {
            await manualWindowUI.ShowStory();
        }

        private async UniTask PushCharactersButton()
        {
            await manualWindowUI.ShowCharacters();
        }

        private async UniTask PushGameScreenButton()
        {
            await manualWindowUI.ShowGameScreen();
        }
    }
}
