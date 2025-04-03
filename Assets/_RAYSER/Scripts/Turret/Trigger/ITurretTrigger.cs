using Turret;

namespace _RAYSER.Scripts.Turret.Trigger
{
    /// <summary>
    /// 弾発射トリガーのインターフェース
    /// </summary>
    public interface ITurretTrigger
    {
        void Initialize(EnemyTurret turret);
    }
}
