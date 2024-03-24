using System;
using _RAYSER.Scripts.Score;
using _RAYSER.Scripts.UI;
using _RAYSER.Scripts.UI.Dialog;
using MessagePipe;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _RAYSER.Scripts.Item
{
    /// <summary>
    /// カスタマイズ画面アイテム購入ボタンUIクラス
    /// </summary>
    public class ItemBuyButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private Image thumbnailImage;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI getText;
        [SerializeField] private Button button;

        private ItemData itemData;
        public int Id { get; set; }

        private IPublisher<DialogOpenSignal> _itemPurchasePublisher;
        private IDisposable _disposable;

        private ScoreData _scoreData;
        private ItemAcquisition _itemAcquisition;

        /// <summary>
        /// ダイアログが開いているかどうか
        /// </summary>
        private bool isDialogOpen = false;

        private void OnEnable()
        {
            MessageBroker.Default.Publish(new UISelectorSignal { forcusUIGameObject = gameObject });
        }

        public void Setup(
            IItem item,
            IPublisher<DialogOpenSignal> itemPurchasePublisher,
            ScoreData scoreData,
            ItemDialog itemDialog,
            ItemAcquisition itemAcquisition
        )
        {
            _itemPurchasePublisher = itemPurchasePublisher;
            _itemAcquisition = itemAcquisition;

            if (item is ItemData data) // セーフキャストを使用
            {
                itemData = data;

                itemNameText.text = itemData.name;
                thumbnailImage.sprite = itemData.iconImage;
                priceText.text = itemData.requiredScore.ToString();

                // スコア変更時にボタンの状態を更新
                _scoreData = scoreData;
                _scoreData.OnScoreChanged += UpdateButtonState;
                UpdateButtonState(scoreData.GetScore());

                itemDialog.OnDialogStateChanged += dialogState =>
                {
                    isDialogOpen = dialogState;
                    UpdateButtonInteractable();
                };

                // 購入ボタン押下時処理
                button.onClick.AddListener(() => OnPurchase(itemData));
            }
            else
            {
                Debug.LogError("提供されたアイテムは ItemData 型ではありません。");
            }
        }

        /// <summary>
        /// 購入ボタン押下時処理
        /// </summary>
        /// <param name="itemData"></param>
        public void OnPurchase(ItemData itemData)
        {
            if (itemData != null)
            {
                _itemPurchasePublisher.Publish(new DialogOpenSignal(itemData));
            }
            else
            {
                Debug.LogError("アイテムデータが設定されていません。");
            }
        }

        /// <summary>
        /// ボタンの有効・無効を更新
        /// </summary>
        private void UpdateButtonInteractable()
        {
            if (button == null)
            {
                Debug.LogError("Button is null.");
                return;
            }

            if (_scoreData == null)
            {
                Debug.LogError("ScoreData is null.");
                return;
            }

            if (itemData == null)
            {
                Debug.LogError("ItemData is null.");
                return;
            }

            bool alreadyPurchased = _itemAcquisition.HasItem(itemData);
            button.interactable =
                !isDialogOpen && !alreadyPurchased && (_scoreData.GetScore() >= itemData.requiredScore);

            // 購入済みの場合は購入済みテキストを表示
            getText.gameObject.SetActive(alreadyPurchased);
        }

        /// <summary>
        /// スコアの更新メソッドでボタンの状態を更新
        /// </summary>
        /// <param name="currentScore"></param>
        private void UpdateButtonState(int currentScore)
        {
            UpdateButtonInteractable();
        }

        private void OnDestroy()
        {
            if (_scoreData != null)
            {
                _scoreData.OnScoreChanged -= UpdateButtonState;
            }
        }
    }
}
