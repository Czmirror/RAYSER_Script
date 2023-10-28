using UnityEngine;

namespace _RAYSER.Scripts.Item
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/New Item")]
    public class ItemData : ScriptableObject, IItem {
        public string itemName;
        public string itemDescription;
        public Sprite iconImage;
        public ItemType itemType;

        public string name => itemName;
        public string description => itemDescription;
        public Sprite icon => iconImage;
        public ItemType itemType => itemType;
    }
}
