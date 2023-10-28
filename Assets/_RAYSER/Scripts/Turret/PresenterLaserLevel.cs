using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Turret
{
    public class PresenterLaserLevel : MonoBehaviour
    {
        [SerializeField] private PlayerLaserLevel _playerLaserLevel;

        /// <summary>
        /// 対象画像のUI
        /// </summary>
        [SerializeField] private Image laserUI;

        /// <summary>
        /// 対応するスピードの値
        /// </summary>
        [SerializeField] private int settingLaserLevel;

        private void Start()
        {
            laserUI.color = new Color32 (255, 255, 255, 0);
            _playerLaserLevel.CurrentLaserLevelObservable
                .Where(x => x == settingLaserLevel)
                .Subscribe(x => reflashUI())
                .AddTo(this);
        }

        private void reflashUI()
        {
            laserUI.color = new Color32 (255, 255, 255, 255);
        }
    }
}
