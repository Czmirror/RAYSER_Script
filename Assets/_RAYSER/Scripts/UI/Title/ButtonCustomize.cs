using Cysharp.Threading.Tasks;
using Event.Signal;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _RAYSER.Scripts.UI.Title
{
    [RequireComponent(typeof(Button))]
    public class ButtonCustomize : MonoBehaviour
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
        /// メニューUI
        /// </summary>
        [SerializeField] private TitleMenuButtonsUI titleMenuButtonsUI;

        /// <summary>
        /// ウィンドウUI
        /// </summary>
        [SerializeField] private CustomizeWindowUI customizeWindowUI;

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
        }

        private void Start()
        {
            // ゲームパッドキャンセルボタン受付
            MessageBroker.Default.Receive<GamePadCancelButtonPush>()
                .Where(_ => _gamePadCancelButtonAcceptance)
                .Subscribe(_ => PushCloseButton())
                .AddTo(this);

        }

        private async UniTask PushButton()
        {
            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);

            await titleMenuButtonsUI.HideUI();
            await customizeWindowUI.ShowUI();
        }

        private async UniTask PushCloseButton()
        {
            // ゲームパッドのキャンセルボタン受付解除
            _gamePadCancelButtonAcceptance = false;

            await customizeWindowUI.HideUI();
            await titleMenuButtonsUI.ShowUI();
        }
    }
}
