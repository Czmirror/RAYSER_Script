using System;
using _RAYSER.Scripts.Item;
using _RAYSER.Scripts.SubWeapon;
using MessagePipe;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    /// <summary>
    /// ゲームシーンの個々のサブウェポンUI
    /// </summary>
    public class SubWeaponIndividual : MonoBehaviour, IDisposable
    {
        private CompositeDisposable _disposable = new CompositeDisposable();
        private ISubscriber<SubweaponMoveDirection> _moveDirectionSubscriber;
        private ItemAcquisition _itemAcquisition;
        private ItemData _itemData;
        [SerializeField] private TextMeshProUGUI nameText;

        [SerializeField] private Image focusImage;

        public void Setup(ItemAcquisition itemAcquisition,
            ISubscriber<SubweaponMoveDirection> moveDirectionSubscriber,
            ItemData itemData)
        {
            _itemAcquisition = itemAcquisition;
            _moveDirectionSubscriber = moveDirectionSubscriber;
            _itemData = itemData;
            nameText.text = _itemData.name;
            // _moveDirectionSubscriber.Subscribe(_ => OnMoveDirectionReceived()).AddTo(_disposable);
            _moveDirectionSubscriber.Subscribe(_ => OnMoveDirectionReceived()).AddTo(this);
            IsMounted(_itemData);
        }

        /// <summary>
        /// サブウェポン装着表示判定
        /// </summary>
        /// <param name="itemData"></param>
        private void IsMounted(ItemData itemData)
        {
            if (_itemAcquisition.GetSelectingSubWeapon() == itemData)
            {
                focusImage.gameObject.SetActive(true);
                // focusImage.color = new Color(focusImage.color.r, focusImage.color.g, focusImage.color.b, 1);
            }
            else
            {
                focusImage.gameObject.SetActive(false);
                // focusImage.color = new Color(focusImage.color.r, focusImage.color.g, focusImage.color.b, 0);
            }
        }

        private void OnMoveDirectionReceived()
        {
            IsMounted(_itemData);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void OnDestroy()
        {
            // Dispose();
        }
    }
}
