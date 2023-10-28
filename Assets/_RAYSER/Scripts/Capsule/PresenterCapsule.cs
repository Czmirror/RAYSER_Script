using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Capsule
{
    /// <summary>
    /// カプセルUI更新用クラス
    /// </summary>
    public class PresenterCapsule : MonoBehaviour
    {
        [SerializeField] private CapsuleStock _capsuleStock;

        /// <summary>
        /// カプセルUI１つ目
        /// </summary>
        [SerializeField] private Image capsuleUI1;

        /// <summary>
        /// カプセルUI２つ目
        /// </summary>
        [SerializeField] private Image capsuleUI2;

        /// <summary>
        /// カプセルUI３つ目
        /// </summary>
        [SerializeField] private Image capsuleUI3;

        /// <summary>
        /// アイテムなし画像
        /// </summary>
        [SerializeField] private Sprite emptySprite;

        /// <summary>
        /// レーザーパワーアップ画像
        /// </summary>
        [SerializeField] private Sprite laserSprite;

        /// <summary>
        /// スピードアップ画像
        /// </summary>
        [SerializeField] private Sprite speedSprite;

        /// <summary>
        /// ボーナススコア画像
        /// </summary>
        [SerializeField] private Sprite bounsSprite;

        /// <summary>
        /// シールド回復画像
        /// </summary>
        [SerializeField] private Sprite recoverSprite;

        /// <summary>
        /// 雑魚敵全滅画像
        /// </summary>
        [SerializeField] private Sprite annihilationSprite;


        private void Start()
        {
            _capsuleStock.CapsuleUI1.Subscribe(x => reflashUI(x, capsuleUI1)).AddTo(this);
            _capsuleStock.CapsuleUI2.Subscribe(x => reflashUI(x, capsuleUI2)).AddTo(this);
            _capsuleStock.CapsuleUI3.Subscribe(x => reflashUI(x, capsuleUI3)).AddTo(this);
        }

        private void reflashUI(CapsuleEnum capsuleEnum, Image capsuleUI)
        {
            switch (capsuleEnum)
            {
                case CapsuleEnum.Empty:
                    capsuleUI.sprite = emptySprite;
                    break;
                case CapsuleEnum.LaserPowerUp:
                    capsuleUI.sprite = laserSprite;
                    break;
                case CapsuleEnum.SpeedUp:
                    capsuleUI.sprite = speedSprite;
                    break;
                case CapsuleEnum.BonusScore:
                    capsuleUI.sprite = bounsSprite;
                    break;
                case CapsuleEnum.ShieldRecover:
                    capsuleUI.sprite = recoverSprite;
                    break;
                case CapsuleEnum.AnnihilationEnemy:
                    capsuleUI.sprite = annihilationSprite;
                    break;
            }
        }
    }
}
