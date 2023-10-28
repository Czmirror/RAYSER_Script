namespace Turret
{
    /// <summary>
    /// レーザーに当たる対象を定義するインターフェース
    /// </summary>
    public interface IHitByLaser
    {
        /// <summary>
        /// 当たったレーザーのクラスを取得
        /// </summary>
        void GetHitLaser(Laser _laser);
    }
}
