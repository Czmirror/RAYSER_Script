using System;
using System.Collections.Generic;
using System.Linq;
using _RAYSER.Scripts.Score;
using _RAYSER.Scripts.SubWeapon;
using Event.Signal;
using MessagePipe;
using UniRx;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// 所有アイテム管理クラス
    /// </summary>
    public class ItemAcquisition : IDisposable
    {
        /// <summary>
        /// 所有中アイテム
        /// </summary>
        private List<ItemData> _items = new List<ItemData>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly ISubscriber<ItemPurchaseSignal> _itemPurchaseSubscriber;
        private readonly ISubscriber<SubweaponMoveDirection> _moveDirectionSubscriber;
        private readonly IPublisher<CurrentSubWeaponIndex> _inputPublisher;
        private readonly ScoreData _scoreData;
        private readonly ItemAcquisition _itemAcquisition;
        private List<ItemData> _subWeapon = new List<ItemData>();
        private int _selectingSubWeaponIndex;

        /// <summary>
        /// 現在選択されているサブウェポン
        /// </summary>
        private ItemData _selectedSubWeapon;

        public ItemAcquisition(ISubscriber<ItemPurchaseSignal> itemPurchaseSubscriber,
            ISubscriber<SubweaponMoveDirection> moveDirectionSubscriber,
            ScoreData scoreData)
        {
            _itemPurchaseSubscriber = itemPurchaseSubscriber;
            _moveDirectionSubscriber = moveDirectionSubscriber;
            _scoreData = scoreData;

            _itemPurchaseSubscriber.Subscribe(OnItemPurchased).AddTo(_disposable);
            _moveDirectionSubscriber.Subscribe(OnMoveDirectionReceived).AddTo(_disposable);
        }

        /**
         * アイテム購入処理
         */
        private void OnItemPurchased(ItemPurchaseSignal signal)
        {
            var itemData = signal.Item as ItemData;
            if (itemData == null)
            {
                // signal.Item が ItemData 型でない場合、処理を中断
                return;
            }

            if (_scoreData.GetScore() >= itemData.requiredScore)
            {
                // スコアが十分な場合、アイテムをリストに追加してスコアを消費させる
                if (HasItem(itemData) == false)
                {
                    _items.Add(itemData);
                    OnItemAdded?.Invoke(itemData);

                    MessageBroker.Default.Publish(new ScoreAccumulation { Score = -itemData.requiredScore });
                }
            }
        }

        /// <summary>
        /// サブウェポン切り替え処理
        /// </summary>
        /// <param name="direction"></param>
        private void OnMoveDirectionReceived(SubweaponMoveDirection direction)
        {
            var subWeapons = GetSubWeapon();
            if (subWeapons.Length == 0) return; // サブウェポンがない場合は何もしない

            int currentIndex = Array.IndexOf(subWeapons, _selectedSubWeapon);
            if (currentIndex == -1) currentIndex = 0; // 現在選択されているサブウェポンがない場合、最初の要素を選択

            // 方向に応じてインデックスを更新
            if (direction == SubweaponMoveDirection.Left)
            {
                currentIndex--;
                if (currentIndex < 0) currentIndex = subWeapons.Length - 1; // リストの最後にループ
            }
            else if (direction == SubweaponMoveDirection.Right)
            {
                currentIndex++;
                if (currentIndex >= subWeapons.Length) currentIndex = 0; // リストの最初にループ
            }

            _selectedSubWeapon = subWeapons[currentIndex] as ItemData;
        }


        public void Dispose()
        {
            _disposable.Dispose();
        }

        /// <summary>
        /// SubWeaponの値を持つデーターを取得する
        /// </summary>
        /// <returns></returns>
        public IItem[] GetSubWeapon()
        {
            return GetItemsByType(ItemType.SubWeapon);
        }

        /// <summary>
        /// Bombの値を持つデーターを取得する
        /// </summary>
        /// <returns></returns>
        public IItem[] GetBomb()
        {
            return GetItemsByType(ItemType.Bomb);
        }

        /// <summary>
        /// Shieldの値を持つデーターを取得する
        /// </summary>
        /// <returns></returns>
        public IItem[] GetShield()
        {
            return GetItemsByType(ItemType.Shield);
        }

        // public IItem GetItemByName(string name) {
        //     return _items.FirstOrDefault(item => item.Name == name);
        // }

        public IItem[] GetItemsByType(ItemType itemType)
        {
            return _items.Where(item => item.itemType == itemType).ToArray();
        }


        // public IItem[] GetAllItems() {
        //     return _items;
        // }

        public event Action<ItemData> OnItemAdded;
        public event Action<ItemData> OnItemRemoved;

        public void AddItem(ItemData item)
        {
            _items.Add(item);
            OnItemAdded?.Invoke(item);
        }

        public void RemoveItem(ItemData item)
        {
            if (_items.Remove(item))
            {
                OnItemRemoved?.Invoke(item);
            }
        }

        /// <summary>
        /// サブウェポンの格納（未使用になるかもしれない）
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void SetSubWeapon(int index, ItemData item)
        {
            if (_subWeapon[index] == null)
            {
                _subWeapon[index] = item;
            }
        }

        /// <summary>
        /// サブウェポンの切り替え処理（未使用になるかもしれない）
        /// </summary>
        /// <param name="direction"></param>
        public void SwitchSubWeapon(SubweaponMoveDirection direction)
        {
            var move = direction == SubweaponMoveDirection.Left ? -1 : 1;
            var index = _selectingSubWeaponIndex + move;
            if (index < 0) index = _subWeapon.Count - 1;
            if (index > _subWeapon.Count - 1) index = 0;
            _selectingSubWeaponIndex = index;

            // メッセージを作成
            var inputParams = new CurrentSubWeaponIndex(_selectingSubWeaponIndex);

            // メッセージ送信
            _inputPublisher.Publish(inputParams);
        }

        /// <summary>
        /// 選択中のサブウェポンを取得する
        /// </summary>
        public ItemData GetSelectingSubWeapon()
        {
            if (_selectedSubWeapon != null)
            {
                return _selectedSubWeapon;
            }
            else
            {
                // サブウェポンが未選択の場合、選択をリセットする
                return ResetSubWeaponSelection();
            }

            // if (_subWeapon[_selectingSubWeaponIndex] == null) return null;
            //
            // return _subWeapon[_selectingSubWeaponIndex];
        }

        /// <summary>
        /// サブウェポン選択をリセットし、最初のサブウェポンを返す（存在する場合）
        /// </summary>
        /// <returns>リセット後の現在のサブウェポン、またはnull</returns>
        public ItemData ResetSubWeaponSelection()
        {
            var subWeapons = GetSubWeapon();
            if (subWeapons.Any())
            {
                // 最初のサブウェポンを現在の選択に設定し、それを返す
                _selectedSubWeapon = subWeapons.First() as ItemData;
                return _selectedSubWeapon;
            }
            else
            {
                // サブウェポンが存在しない場合はnullを返す
                return null;
            }
        }

        /// <summary>
        /// 特定のアイテムが所持リストに含まれているか確認するメソッド
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool HasItem(ItemData item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        /// _itemsが一つ以上あるか判定するメソッド
        /// </summary>
        /// <returns></returns>
        public bool HasItem()
        {
            return _items.Count > 0;
        }

        public void debugAddItem(ItemData item)
        {
            if (item == null) return;
            if (_items.Contains(item)) return;

            _items.Add(item);
        }
    }
}
