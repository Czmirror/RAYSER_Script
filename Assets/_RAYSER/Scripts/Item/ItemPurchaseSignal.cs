using _RAYSER.Scripts.Commodity;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテム購入シグナルクラス
    /// </summary>
    public class ItemPurchaseSignal
    {
        public IItem Item { get; private set; }
        // public ICommodity Commodity { get; private set; }

        public ItemPurchaseSignal(IItem item)
        {
            Item = item;
        }
    }

}
