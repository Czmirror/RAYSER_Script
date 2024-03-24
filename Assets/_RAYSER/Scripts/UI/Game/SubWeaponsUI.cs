using _RAYSER.Scripts.Item;
using UnityEngine;

namespace UI.Game
{
    /// <summary>
    /// ゲームシーンのサブウェポンUI表示処理
    /// </summary>
    public class SubWeaponsUI : MonoBehaviour
    {
        private ItemAcquisition _itemAcquisition;

        public void Setup(ItemAcquisition itemAcquisition)
        {
            _itemAcquisition = itemAcquisition;
            gameObject.SetActive(_itemAcquisition.HasItem());
        }
    }
}
