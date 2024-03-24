using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// アイテム購入監視クラス
    /// </summary>
    public class ItemPurchaseSubscriber : MonoBehaviour
    {
        private ISubscriber<ItemPurchaseSignal> _subscriber;
        readonly CompositeDisposable disposable = new CompositeDisposable();

        [Inject]
        public void Construct(ISubscriber<ItemPurchaseSignal> subscriber)
        {
            _subscriber = subscriber;
        }

        private void Start()
        {
            // ItemPurchaseSignalを購読し、購入処理を行う
            // DisposableBag
            //
            //     .AddTo(disposable);
        }

        private void PurchaseItem(ItemPurchaseSignal signal)
        {
            // アイテム購入処理
            Debug.Log($"Purchased item: {signal.Item.name}");
            // Debug.Log($"Purchased item: {signal.Item.name}, Cost: {signal.Commodity.requiredScore}");
            // 他の処理...
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }

}
