using System;
using Turret;
using UnityEngine;
using UnityEngine.UI;

namespace Target
{
    public class LockonMaker : MonoBehaviour
    {
        /// <summary>
        /// 自身のRectTransform
        /// </summary>
        [SerializeField] private RectTransform rectTransform;

        /// <summary>
        /// カーソルのImage
        /// </summary>
        [SerializeField] private Image image;

        /// <summary>
        /// ロックオン対象のTransform
        /// </summary>
        [SerializeField] private Transform LockonTarget;

        [SerializeField] private PlayerLaserTurret _playerLaserTurret;

        private void Start()
        {
            image.enabled = false;
        }

        private void Update()
        {
            if (_playerLaserTurret.CurrentPlayerTargeting().IsTarget() == false)
            {
                image.enabled = false;
                return;
            }

            // ターゲットのtransform取得
            LockonTarget = _playerLaserTurret.CurrentPlayerTargeting().CurrentTargetGameObject().transform;

            // ターゲットが存在する場合ロックオンマーカー表示
            if (LockonTarget.position != null)
            {
                image.enabled = true;
                rectTransform.Rotate(0, 0, 1f);
                Vector3 targetPoint = Camera.main.WorldToScreenPoint(LockonTarget.position);
                rectTransform.position = targetPoint;
            }
        }
    }
}
