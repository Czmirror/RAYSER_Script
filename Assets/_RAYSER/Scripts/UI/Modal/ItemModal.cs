using _RAYSER.Scripts.Item;
using UnityEditor;
using UnityEngine;

namespace _RAYSER.Scripts.UI.Modal
{
    /// <summary>
    /// ItemModal固有の表示・非表示処理
    /// </summary>
    public class ItemModal : MonoBehaviour, IModal
    {
        [SerializeField] private ItemList itemList;
        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private Transform contentTransform;

        private void Start() {
            foreach (var item in itemList.items) {
                var instance = Instantiate(itemButtonPrefab, contentTransform);
                var itemButton = instance.GetComponent<ItemBuyButton>();
                itemButton.Setup(item, OnItemPurchaseConfirmation);
            }
        }

        private void OnItemPurchaseConfirmation(ItemData item) {
            Debug.Log(item.name);
        }
        public void Show()
        {
            // ItemModal固有の表示処理
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            // ItemModal固有の非表示処理
            this.gameObject.SetActive(false);
        }

        public bool IsActive
        {
            get { return this.gameObject.activeSelf; }
        }
    }
}
