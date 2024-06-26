using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _RAYSER.Scripts.UI.Modal
{
    /// <summary>
    /// EquipmentModal固有の表示・非表示処理
    /// </summary>
    public class EquipmentModal : MonoBehaviour, IModal
    {
        public async UniTask Show()
        {
            // EquipmentModal固有の表示処理
            this.gameObject.SetActive(true);
        }

        public async UniTask Hide()
        {
            // EquipmentModal固有の非表示処理
            this.gameObject.SetActive(false);
        }

        public bool IsActive {
            get { return this.gameObject.activeSelf; }
        }
    }
}
