using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _RAYSER.Scripts.Turret.ShootingEffect
{
    /// <summary>
    /// 発射エフェクトを管理するインターフェース
    /// </summary>
    public interface IShootingEffect
    {
        /// <summary>
        /// エフェクトの初期化
        /// </summary>
        /// <param name="parentTransform">エフェクトを追従させる親のTransform</param>
        void Initialize(Transform parentTransform);

        /// <summary>
        /// エフェクトの再生
        /// </summary>
        UniTask PlayEffectAsync(float duration);

        /// <summary>
        /// エフェクトの停止
        /// </summary>
        void StopEffect();
    }
}
