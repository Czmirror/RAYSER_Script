using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace PlayerMove
{
    public class PresenterSpeedLevel : MonoBehaviour
    {
        /// <summary>
        /// 自機移動用クラス
        /// </summary>
        [SerializeField] private PlayerMoveCore playerMoveCore;

        /// <summary>
        /// 対象画像のUI
        /// </summary>
        [SerializeField] private Image speedUI;

        /// <summary>
        /// 対応するスピードの値
        /// </summary>
        [SerializeField] private int settingSpeedLevel;

        private void Start()
        {
            speedUI.color = new Color32 (255, 255, 255, 0);
            playerMoveCore.currentSpeedLevelObservable
                .Where(x => x == settingSpeedLevel)
                .Subscribe(x => reflashUI())
                .AddTo(this);
        }

        private void reflashUI()
        {
            speedUI.color = new Color32 (255, 255, 255, 255);
        }
    }
}
