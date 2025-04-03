using UnityEngine;

namespace _RAYSER.Scripts.UI
{
    /// <summary>
    /// RectTransformの位置計算クラス
    /// </summary>
    public static class UIPositionCalculator
    {
        /// <summary>
        /// 指定したRectTransformの親RectTransformにおける中央のワールド座標を返します。
        /// </summary>
        public static Vector3 CalculateCenterPosition(RectTransform rectTransform)
        {
            if (rectTransform == null || rectTransform.parent == null)
            {
                Debug.LogWarning("RectTransformまたはその親がnullです。");
                return Vector3.zero;
            }

            // 親のRectTransformを取得
            RectTransform parentRect = rectTransform.parent as RectTransform;
            // 親のRectの中心（ローカル座標）を取得
            Vector2 parentCenter = parentRect.rect.center;
            // ローカル座標をワールド座標に変換して返す
            return parentRect.TransformPoint(parentCenter);
        }
    }
}
