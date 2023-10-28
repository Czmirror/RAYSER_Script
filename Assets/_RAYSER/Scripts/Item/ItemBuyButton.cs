using UnityEngine;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// カスタマイズ画面アイテム購入ボタンUI
    /// </summary>
    public class ItemBuyButton : MonoBehaviour
    {
        public int Id { get; set; }
        private void Start()
        {
            // MessagePipe受信処理
        }

        public void Setup(IItem item)
        {
            // モーダル内UIのフォーカス無効化
            // アイテム情報を設定
            // アイテム名を購入しますか表示
            // 購入ボタンを押したら
            // MessagePipe送信
            // YesNoダイアログ非表示

        }
    }
}
