using _RAYSER.Scripts.Item;

namespace _RAYSER.Scripts.Commodity
{
    /// <summary>
    /// 商品用インターフェース
    /// </summary>
    public interface ICommodity
    {
        /// <summary>
        /// 必要スコア
        /// </summary>
        int requiredScore { get; }

        ItemPurchaseProcessing _itemPurchaseProcessing { get; }

        /// <summary>
        /// スコア交換処理
        /// </summary>
        void ExchangeScore();
    }
}
