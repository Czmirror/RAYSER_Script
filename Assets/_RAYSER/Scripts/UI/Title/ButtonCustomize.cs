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
        }

        private async UniTask PushButton()
        {
            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);

            await titleMenuButtonsUI.HideUI();
            await customizeWindowUI.ShowUI();
        }
    }
}
