using System.Collections.Generic;
using UnityEngine;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテムリストScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "ItemList", menuName = "Items/New Item List")]
    public class ItemList : ScriptableObject {
        public List<ItemData> items;
    }
}
