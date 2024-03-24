using _RAYSER.Scripts.Item;

namespace _RAYSER.Scripts.SubWeapon
{
    /// <summary>
    /// サブウェポン使用シグナル
    /// </summary>
    public class SubweaponUseSignal
    {
        public SubweaponUseType SubweaponUseType { get; }
        public SubweaponUseSignal(SubweaponUseType subweaponUseType)
        {
            SubweaponUseType = subweaponUseType;
        }
    }

    public enum SubweaponUseType
    {
        Use,
        Stop,
    }
}
