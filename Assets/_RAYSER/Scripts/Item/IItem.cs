using UnityEngine.UI;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテム用インターフェース
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// アイテム名
        /// </summary>
        string name { get; }

        /// <summary>
        /// アイテム説明
        /// </summary>
        string description { get; }

        /// <summary>
        /// アイテムアイコン
        /// </summary>
        Image iconImage { get; }

        /// <summary>
        /// アイテム種別
        /// </summary>
        ItemType itemType { get; }
    }
}
