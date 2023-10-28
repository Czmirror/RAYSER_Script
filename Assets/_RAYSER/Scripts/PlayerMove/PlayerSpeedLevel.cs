using UnityEngine;

namespace PlayerMove
{
    /// <summary>
    /// 自機のスピードレベル
    /// </summary>
    public class PlayerSpeedLevel
    {
        [SerializeField] private int speedLevel = 1;
        [SerializeField] private int maxSpeedLevel = 3;

        /// <summary>
        /// 現在のスピードレベルを返却
        /// </summary>
        /// <returns>現在のスピードレベル</returns>
        public int CurrentSpeedLevel()
        {
            return speedLevel;
        }

        /// <summary>
        /// スピードアップ
        /// </summary>
        public void SpeedLevelUp()
        {
            if (speedLevel == maxSpeedLevel)
            {
                return;
            }

            speedLevel += 1;
        }
    }
}
