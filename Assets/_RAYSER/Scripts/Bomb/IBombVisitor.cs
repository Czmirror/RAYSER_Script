using System;
using UnityEngine;

namespace _RAYSER.Scripts.Bomb
{
    /// <summary>
    /// ボムのVisitorインターフェース
    /// </summary>
    public interface IBombVisitor
    {
        void Visit(BombAction action);

        bool CanUse();
        void Use(Vector3 position);
        void Reset();

        /// <summary>
        /// 使用回数
        /// </summary>
        int UseCount { get; }

        /// <summary>
        /// 使用回数変更イベント
        /// </summary>
        event Action<int> OnUseCountChanged;
    }
}
