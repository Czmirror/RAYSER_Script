using UnityEngine;
using UnityEngine.UI;

namespace Capsule
{
    public class CapsuleTextFollower: MonoBehaviour
    {
        public Transform target; // 追従するターゲットのTransform
        public Vector3 offset = new Vector3(0, 2.0f, 0); // ターゲットからのオフセット
        public Text capsuleText; // カプセル情報を表示するText

        private void Update()
        {
            // ターゲットが設定されていなければ何もしない
            if (target == null) return;

            // ターゲットの位置にオフセットを加えた位置にUIを配置
            transform.position = target.position + offset;

            // カメラの方向を向くようにする
            transform.LookAt(Camera.main.transform);
        }

        // カプセル情報を更新するメソッド
        public void SetCapsuleInfo(ICapsuleinfo capsuleInfo)
        {
            capsuleText.text = capsuleInfo.Name;
            // target = capsuleInfo.transform;
        }
    }
}
