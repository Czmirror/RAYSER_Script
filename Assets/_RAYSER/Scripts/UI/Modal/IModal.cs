namespace _RAYSER.Scripts.UI.Modal
{
    /// <summary>
    /// モーダルインターフェース
    /// </summary>
    public interface IModal {
        /// <summary>
        /// モーダル表示
        /// </summary>
        void Show();

        /// <summary>
        /// モーダル非表示
        /// </summary>
        void Hide();

        /// <summary>
        /// 表示状態
        /// </summary>
        bool IsActive { get; }
    }
}
