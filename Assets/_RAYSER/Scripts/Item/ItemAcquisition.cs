using System;
using System.Collections.Generic;
using System.Linq;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// 所有アイテム管理クラス
    /// </summary>
    public class ItemAcquisition
    {
        /// <summary>
        /// 所有中アイテム
        /// </summary>
        private List<IItem> _items = new List<IItem>();

        private void Start() {
            itemPurchaseSubscriber.Subscribe(OnItemPurchased).AddTo(this);
        }

        private void OnItemPurchased(ItemPurchaseSignal signal) {
            // アイテム購入処理
        }

        /// <summary>
        /// アイテム格納処理
        /// </summary>
        /// <param name="items"></param>
        // public void SetItem(IItem[] items)
        // {
        //     _items = items;
        // }

        /// <summary>
        /// SubWeaponの値を持つデーターを取得する
        /// </summary>
        /// <returns></returns>
        public IItem[] GetSubWeapon() {
            return GetItemsByType(ItemType.SubWeapon);
        }

        /// <summary>
        /// Bombの値を持つデーターを取得する
        /// </summary>
        /// <returns></returns>
        public IItem[] GetBomb() {
            return GetItemsByType(ItemType.Bomb);
        }

        /// <summary>
        /// Shieldの値を持つデーターを取得する
        /// </summary>
        /// <returns></returns>
        public IItem[] GetShield() {
            return GetItemsByType(ItemType.Shield);
        }

        // public IItem GetItemByName(string name) {
        //     return _items.FirstOrDefault(item => item.Name == name);
        // }

        public IItem[] GetItemsByType(ItemType itemType) {
            return _items.Where(item => item.itemType == itemType).ToArray();
        }


        // public IItem[] GetAllItems() {
        //     return _items;
        // }

        public event Action<IItem> OnItemAdded;
        public event Action<IItem> OnItemRemoved;

        public void AddItem(IItem item) {
            _items.Add(item);
            OnItemAdded?.Invoke(item);
        }

        public void RemoveItem(IItem item) {
            if (_items.Remove(item)) {
                OnItemRemoved?.Invoke(item);
            }
        }
    }
}
