using UnityEngine;

namespace _RAYSER.Scripts.Event.Signal
{
    /// <summary>
    /// ローリングシグナル
    /// </summary>
    public class RollingSignal
    {
        public Vector3 RotationAxis;  // 回転の軸
        public float RotationAngle;   // 回転角度
        public float? Duration;       // 回転時間 (null の場合、デフォルト値を使用)

        public RollingSignal(Vector3 axis, float angle, float? duration = null)
        {
            RotationAxis = axis;
            RotationAngle = angle;
            Duration = duration;
        }
    }
}
