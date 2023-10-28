using _RAYSER.Scripts.Commodity;
using _RAYSER.Scripts.Item;
using UnityEngine.UI;

namespace _RAYSER.Scripts.Weapon
{
    public class Vulcan : IItem, ICommodity
    {
        public string name => "バルカン";
        public string description => "前方へ連続攻撃ができるバルカン砲";
        public Image iconImage => null;
        public ItemType itemType => ItemType.SubWeapon;
        public int requiredScore => 10000;

        public ItemPurchaseProcessing _itemPurchaseProcessing => new ItemPurchaseProcessing();

        public void ExchangeScore()
        {
            _itemPurchaseProcessing.BuyItem(this, this);
        }
    }
}
