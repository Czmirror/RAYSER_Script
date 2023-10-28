using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _RAYSER.Scripts.UI.Title
{
    [RequireComponent(typeof(Button))]
    public class ButtonStart : MonoBehaviour
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
        /// ゲームスタートUI
        /// </summary>
        [SerializeField] private MissionStartWindowUI missionStartWindowUI;

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

        public async UniTask PushButton()
        {
            //初期選択ボタンの初期化
            EventSystem.current.SetSelectedGameObject(null);

            await titleMenuButtonsUI.HideUI();
            await missionStartWindowUI.ShowUI();
            await missionStartWindowUI.HideUI();
        }
    }
}
