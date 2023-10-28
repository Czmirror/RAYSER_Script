using UnityEngine;

namespace PlayerMove
{
    /// <summary>
    /// 移動用インターフェース
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// 初期設定
        /// </summary>
        /// <param name="playerMoveCore">自機移動用クラス</param>
        void Initialize(PlayerMoveCore playerMoveCore);

        /// <summary>
        /// 移動処理
        /// </summary>
        void Move();

        /// <summary>
        /// コントローラーの移動情報
        /// </summary>
        /// <param name="moveValue">移動値</param>
        void SetDirection(Vector2 moveValue);

        /// <summary>
        /// 移動制限
        /// </summary>
        void MovementRestrictions();
    }
}
