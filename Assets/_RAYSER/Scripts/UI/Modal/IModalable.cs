using Cysharp.Threading.Tasks;

namespace _RAYSER.Scripts.UI.Modal
{
    /// <summary>
    /// モーダル利用インターフェース
    /// </summary>
    public interface IModalable {
        UniTask ToggleModal(IModal modal);
    }
}
