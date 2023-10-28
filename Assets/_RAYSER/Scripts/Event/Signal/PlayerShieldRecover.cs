namespace Event.Signal
{
    /// <summary>
    /// 自機シールド回復通知処理
    /// </summary>
    public class PlayerShieldRecover
    {
        /// <summary>
        /// シールド回復値
        /// </summary>
        public readonly float ShieldRecoverPoint;

        /// <summary>
        /// シールド回復値設定
        /// </summary>
        /// <param name="shieldRecoverPoint">シールド回復値</param>
        public PlayerShieldRecover(float shieldRecoverPoint)
        {
            ShieldRecoverPoint = shieldRecoverPoint;
        }

    }
}
