using Cysharp.Threading.Tasks;

namespace _RAYSER.Scripts.UI
{
    /// <summary>
    /// ウィンドウ用インターフェース
    /// </summary>
    public interface IWindowUI
    {
        UIActiveSetter UIActiveSetter { get; set; }

        /// <summary>
        /// UIの初期化
        /// </summary>
        // void InitializeUI();

        /// <summary>
        /// UIの有効・無効設定
        /// </summary>
        /// <param name="isActive"></param>
        void SetActive(bool isActive);

        UniTask ShowUI();

        UniTask HideUI();
    }
}
