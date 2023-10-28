using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UniRx;

namespace Shield
{
    /// <summary>
    /// 自機のシールドパーセント数値表示処理
    /// </summary>
    public class PresenterPlayerShieldPercent : MonoBehaviour
    {
        /// <summary>
        /// 監視中のプレイヤーのシールド
        /// </summary>
        [SerializeField] private PlayerShield _playerShield;

        /// <summary>
        /// 更新対象UI
        /// </summary>
        [SerializeField] private TextMeshProUGUI shieldUI;

        /// <summary>
        /// UIアニメーション時間
        /// </summary>
        [SerializeField] private float tweenTime = 0.5f;

        /// <summary>
        /// 自機の最大シールド値
        /// </summary>
        private float maxShield;

        private void Start()
        {
            _playerShield.maxShieldObservable.Subscribe(x => maxShield = x).AddTo(this);
            _playerShield.ShieldObservable.Subscribe(x => RefreshUI(x)).AddTo(this);
        }

        private void RefreshUI(float shield)
        {
            float valueFrom = float.Parse(shieldUI.text, CultureInfo.InvariantCulture.NumberFormat);
            float valueTo = (shield / maxShield) * 100;
            var shieldUITween = DOTween.To(
                () => valueFrom,
                x => {
                    shieldUI.text = x.ToString("F0");
                },
                valueTo,
                tweenTime
            );

            shieldUITween
                .Pause()
                .SetAutoKill(false)
                .SetLink(gameObject)
                .Restart();
        }
    }
}
