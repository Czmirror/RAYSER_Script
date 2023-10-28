using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _RAYSER.Scripts.UI
{
    /// <summary>
    /// タイトルメニューボタンUIクラス
    /// </summary>
    public class TitleMenuButtonsUI : MonoBehaviour, IWindowUI
    {
        UIActiveSetter _uiActiveSetter = new UIActiveSetter();
        private IWindowUI _iuiImplementation;

        UIEffect _uiEffect = new UIEffect();
        private IUIEffect _ieffectImplementation;

        private CancellationTokenSource cts = new CancellationTokenSource();

        [SerializeField] private CanvasGroup menuButtonsCanvasGroup;

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

        public void SetActive(bool isActive)
        {
            _uiActiveSetter.SetActive(gameObject, isActive);
        }

        public async UniTask ShowUI()
        {
            try
            {
                SetActive(true);

                // メニューボタンのフェードイン
                await _uiEffect.FadeIn(menuButtonsCanvasGroup, cts.Token);

                //初期選択ボタンの再指定
                EventSystem.current.SetSelectedGameObject(firstFocusUI);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        public async UniTask HideUI()
        {
            try
            {
                // メニューボタンのフェードアウト
                await _uiEffect.FadeOut(menuButtonsCanvasGroup, cts.Token);

                SetActive(false);

                //初期選択ボタンの初期化
                EventSystem.current.SetSelectedGameObject(null);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }
    }
}
