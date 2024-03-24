using _RAYSER.Scripts.SubWeapon;
using _Vendor.baba_s.SubclassSelector;
using UnityEngine;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテムデータークラス
    /// </summary>
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/New Item")]
    public class ItemData : ScriptableObject, IItem
    {
        public string itemName;
        public string itemDescription;
        public Sprite icon;
        public int _requiredScore;

        [SerializeField] private ItemType _itemType;

        public string name => itemName;
        public string description => itemDescription;
        public Sprite iconImage => icon;
        public ItemType itemType => _itemType;

        /// <summary>
        /// サブウェポンインターフェース
        /// </summary>
        [SerializeReference, SubclassSelector(true)]
        ISubWeaponVisitor SubWeaponVisitor;

        public int requiredScore
        {
            get { return _requiredScore; }
        }

        public ISubWeaponVisitor subWeaponVisitor
        {
            get { return SubWeaponVisitor; }
        }
    }
}
