using System;
using Cysharp.Threading.Tasks;
using Turret;
using UnityEngine;

namespace _RAYSER.Scripts.Turret.Trigger
{
    [Serializable]
    public class ChargedFireTrigger : ITurretTrigger
    {
        [SerializeField] private GameObject chargeEffectPrefab; // エネルギー充填エフェクトのPrefab
        private GameObject chargeEffectInstance; // 実際のエフェクトインスタンス
        private EnemyTurret turret;

        public void Initialize(EnemyTurret turret)
        {
            this.turret = turret;

            // エフェクトの初期化
            if (chargeEffectPrefab != null)
            {
                Transform parentTransform = turret.transform.root; // 親であるEnemyShipのTransform
                chargeEffectInstance = UnityEngine.Object.Instantiate(chargeEffectPrefab, parentTransform);
                chargeEffectInstance.transform.localPosition = Vector3.zero; // 中心位置に配置
                chargeEffectInstance.SetActive(false); // 初期状態では非表示
            }

            // エネルギー充填エフェクトのサイクルを開始
            ChargeEffectCycle().Forget();

            // 弾の発射をそのまま継続
            turret.StartShootingAsync();
        }

        private async UniTaskVoid ChargeEffectCycle()
        {
            while (turret != null) // タレットが存在する間は繰り返す
            {
                // 充填エフェクトの表示
                if (chargeEffectInstance != null)
                {
                    chargeEffectInstance.SetActive(true);
                    await AnimateChargeEffect(); // 充填エフェクトのアニメーション
                    chargeEffectInstance.SetActive(false);
                }
            }
        }

        private async UniTask AnimateChargeEffect()
        {
            if (chargeEffectInstance == null) return;

            float elapsed = 0f;
            float chargeTime = turret.ShotInterval; // 弾の発射間隔と同じ周期で充填エフェクトを実行
            float maxScale = 4f; // 最大スケール値
            float initialScale = 0.5f;

            // スケールを徐々に大きくする
            while (elapsed < chargeTime)
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
