using _RAYSER.Scripts.Commodity;
using Event.Signal;
using MessagePipe;
using UniRx;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテム購入手続きクラス（ItemAcquisitionからアイテム購入部分を独立させるか検討中）
    /// </summary>
    public class ItemPurchaseProcessing
    {
        private readonly IPublisher<ItemData> _onDoITemEvent;
        private readonly ISubscriber<ItemData> _onReceiveITemEvent;

        /// <summary>
        /// アイテム購入処理
        /// </summary>
        /// <param name="item"></param>
        /// <param name="commodity"></param>
        public void BuyItem(IItem item, ICommodity commodity)
        {
            // アイテム購入処理
            // var score = commodity.requiredScore * -1;
            // MessageBroker.Default.Publish(new ScoreAccumulation{Score = score});

            // アイテム種別に応じて処理を分岐
            // SubWeaponの場合
            if (item.itemType == ItemType.SubWeapon)
            {
            }
            // Bombの場合
            else if (item.itemType == ItemType.Bomb)
            {
            }
            // Shieldの場合
            else if (item.itemType == ItemType.Shield)
            {
            }
        }
    }
}
