using _RAYSER.Scripts.Item.ItemAction;
using _Vendor.baba_s.SubclassSelector;
using UnityEngine;


namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテムのコアクラス（未使用になるかもしれない）
    /// </summary>
    public class ItemCore : MonoBehaviour
    {
        [SerializeReference, SubclassSelector(true)]
        private IItemAction _itemAction;

        /// <summary>
        /// アイテム処理実行
        /// </summary>
        public void ItemAction()
        {
            _itemAction.Execute();
        }
    }
}
