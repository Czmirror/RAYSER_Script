using _RAYSER.Scripts.Commodity;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテム購入シグナル
    /// </summary>
    public class ItemPurchaseSignal
    {
        public IItem Item { get; private set; }
        public ICommodity Commodity { get; private set; }

        public ItemPurchaseSignal(IItem item, ICommodity commodity)
        {
            Item = item;
            Commodity = commodity;
        }
    }

}
