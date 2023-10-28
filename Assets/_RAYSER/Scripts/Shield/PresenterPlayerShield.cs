using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Shield
{
    /// <summary>
    /// 自機のシールドゲージ表示処理
    /// </summary>
    public class PresenterPlayerShield : MonoBehaviour
    {
        /// <summary>
        /// 監視中のプレイヤーのシールド
        /// </summary>
        [SerializeField] private PlayerShield _playerShield;

        /// <summary>
        /// 更新対象UI
        /// </summary>
        [SerializeField] private Image shieldUI;

        /// <summary>
        /// UIアニメーション時間
        /// </summary>
        [SerializeField] private float tweenTime = 1;

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
            float valueFrom = shieldUI.fillAmount;
            float valueTo = shield / maxShield;
            var shieldUITween = DOTween.To(
                () => valueFrom,
                x => {
                    shieldUI.fillAmount = x;
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
