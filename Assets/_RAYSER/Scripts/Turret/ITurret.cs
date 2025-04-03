using Cysharp.Threading.Tasks;

namespace Turret
{
    /// <summary>
    /// 砲台インターフェース
    /// </summary>
    public interface ITurret
    {
        /// <summary>
        /// 初期化処理
        /// </summary>
        void Initialize();

        /// <summary>
        /// 射撃開始処理
        /// </summary>
        UniTask StartShootingAsync();

        /// <summary>
        /// 射撃停止処理
        /// </summary>
        void StopShooting();

        /// <summary>
        /// リソース解放処理 (非同期)
        /// </summary>
        UniTask CleanupAsync();
    }
}
