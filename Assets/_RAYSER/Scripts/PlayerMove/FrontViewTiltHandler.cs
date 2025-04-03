using UnityEngine;

namespace PlayerMove
{
    /// <summary>
    /// フロントビューの傾きを管理
    /// </summary>
    /// <summary>
    /// フロントビューの傾きを管理
    /// </summary>
    public class FrontViewTiltHandler : MonoBehaviour
    {
        [SerializeField] private float maxTiltAngle = 45f; // 最大傾き角度
        [SerializeField] private float tiltSpeed = 5f;     // 傾き適用速度
        private Quaternion targetRotation;

        private void Start()
        {
            targetRotation = transform.localRotation;
        }

        /// <summary>
        /// スティックの入力値に応じて傾きを適用
        /// </summary>
        public void ApplyTilt(float moveDirectionX)
        {
            float tiltAngle = Mathf.Lerp(0, maxTiltAngle, Mathf.Abs(moveDirectionX)); // スティックの傾きに応じて角度を調整
            tiltAngle *= Mathf.Sign(moveDirectionX); // 左右の方向を考慮

            targetRotation = Quaternion.Euler(tiltAngle, 0, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
    }
}
