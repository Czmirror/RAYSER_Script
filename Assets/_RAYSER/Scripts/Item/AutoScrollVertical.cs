using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace _RAYSER.Scripts.Item
{
    public class AutoScrollVertical: MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect; // インスペクターでセット

        /// <summary>
        /// 選択されたアイテム(target)が常にviewport内に表示されるよう、縦方向のスクロール位置を調整します。
        /// </summary>
        public void ScrollToSelectedItem(RectTransform target)
        {
            // レイアウトを最新にしておかないと、座標計算がずれる可能性がある
            Canvas.ForceUpdateCanvases();

            RectTransform content = scrollRect.content;
            RectTransform viewport = scrollRect.viewport;

            // --- (1) アイテムの四隅ワールド座標を取得 ---
            Vector3[] itemCorners = new Vector3[4];
            target.GetWorldCorners(itemCorners);

            // --- (2) ビューポート空間に変換 ---
            for (int i = 0; i < 4; i++)
            {
                // InverseTransformPointで「ビューポート座標系」に変換する
                itemCorners[i] = viewport.InverseTransformPoint(itemCorners[i]);
            }

            // itemCorners は以下の順序で格納される:
            // 0: bottom-left, 1: top-left, 2: top-right, 3: bottom-right
            float itemTop    = itemCorners[1].y; // top-left のY
            float itemBottom = itemCorners[0].y; // bottom-left のY

            float viewportTop    = viewport.rect.yMax;  // ビューポートの上端
            float viewportBottom = viewport.rect.yMin;  // ビューポートの下端

            // --- (3) 上下端のはみ出しをチェックして offset を計算 ---
            float offset = 0f;
            // 上端がビューポート上端より上に飛び出しているなら
            if (itemTop > viewportTop)
            {
                offset = itemTop - viewportTop;
            }
            // 下端がビューポート下端より下に飛び出しているなら
            else if (itemBottom < viewportBottom)
            {
                offset = itemBottom - viewportBottom;
            }

            // すでに表示領域内に収まっていれば何もしない
            if (Mathf.Approximately(offset, 0f)) return;

            // --- (4) offsetをnormalized座標に変換して反映 ---
            float contentHeight = content.rect.height;
            float viewportHeight = viewport.rect.height;
            float normalizedOffset = offset / (contentHeight - viewportHeight);

            // verticalNormalizedPosition は 1=最上部、0=最下部 なので
            float newNormalizedPos = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + normalizedOffset);
            scrollRect.verticalNormalizedPosition = newNormalizedPos;
        }
    }
}
