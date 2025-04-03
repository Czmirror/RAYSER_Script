using TMPro;
using UnityEngine;

namespace _RAYSER.Scripts.Item.Camera
{
    /// <summary>
    /// アイテム名表示UI
    /// </summary>
    public class ItemNameDisplay : MonoBehaviour
    {
        [SerializeField] private string itemName;
        [SerializeField] private TMP_Text _text;
        private UnityEngine.Camera _camera;

        private void Start()
        {
            // カメラ未設定の場合MainCameraを取得
            if (_camera == null)
            {
                _camera = UnityEngine.Camera.main;
            }

            // itemNameが空の場合はGameObjectの名前を表示
            if (string.IsNullOrEmpty(itemName))
            {
                itemName = gameObject.name;
            }

            // カメラが設定されている場合
            if (_camera != null)
            {
                _text.text = itemName;
            }
        }

        private void Update()
        {
            // カメラが設定されている場合
            if (_camera != null)
            {
                // カメラの方向に向けて常に表示
                transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
            }
        }
    }
}
