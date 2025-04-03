using System;
using Turret;
using UnityEngine;

namespace _RAYSER.Scripts.Turret.Trigger
{
    /// <summary>
    /// 即時発射トリガー
    /// </summary>
    [Serializable]
    public class ImmediateFireTrigger : ITurretTrigger
    {
        public void Initialize(EnemyTurret turret)
        {
            turret.StartShootingAsync();
        }
    }
}
