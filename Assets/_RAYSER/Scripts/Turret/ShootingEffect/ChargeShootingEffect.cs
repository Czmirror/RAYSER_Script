using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _RAYSER.Scripts.Turret.ShootingEffect
{
    /// <summary>
    /// チャージショットのエフェクト
    /// </summary>
    [Serializable]
    public class ChargeShootingEffect : IShootingEffect
    {
        /// <summary>
        /// エフェクトのPrefab
        /// </summary>
        [SerializeField] private GameObject chargeEffectPrefab;

        /// <summary>
        /// エフェクトの最大スケール値
        /// </summary>
        [SerializeField] private float maxScale = 4f;

        /// <summary>
        /// エフェクトの初期スケール値
        /// </summary>
        [SerializeField] private float initialScale = 0.5f;

        /// <summary>
        /// 実際のエフェクトインスタンス
        /// </summary>
        private GameObject chargeEffectInstance;

        /// <summary>
        /// エフェクトを追従させる親のTransform
        /// </summary>
        private Transform parentTransform;

        /// <summary>
        /// アニメーションを停止するためのフラグ
        /// </summary>
        private bool isAnimating;

        public void Initialize(Transform parentTransform)
        {
            this.parentTransform = parentTransform;

            if (chargeEffectPrefab != null)
            {
                chargeEffectInstance = Object.Instantiate(chargeEffectPrefab, parentTransform);
                chargeEffectInstance.transform.localPosition = Vector3.zero; // 敵の中心に配置
            }
        }

        public async UniTask PlayEffectAsync(float duration)
        {
            if (chargeEffectInstance == null || isAnimating) return;

            isAnimating = true;

            while (isAnimating && parentTransform != null)
            {
                chargeEffectInstance.SetActive(true);

                // アニメーションの実行
                await AnimateChargeEffect(duration);

                // スケールを初期化して非表示
                chargeEffectInstance.transform.localScale = new Vector3(initialScale, initialScale, initialScale);
                chargeEffectInstance.SetActive(false);

                // 次のフレームまで待機
                await UniTask.Yield();
            }
        }

        public void StopEffect()
        {
            isAnimating = false;

            if (chargeEffectInstance != null)
            {
                chargeEffectInstance.SetActive(false);
            }
        }

        private async UniTask AnimateChargeEffect(float chargeTime)
        {
            if (chargeEffectInstance == null) return;

            float elapsed = 0f;

            // スケールを徐々に大きくする
            while (elapsed < chargeTime && isAnimating)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / chargeTime;
                float currentScale = Mathf.Lerp(initialScale, maxScale, t);
                chargeEffectInstance.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
                await UniTask.Yield();
            }
        }
    }
}
